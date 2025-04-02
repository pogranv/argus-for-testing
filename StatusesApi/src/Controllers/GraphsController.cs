using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusesApi.Exceptions;
using StatusesApi.Models.View.Requests;
using StatusesApi.Models.View.Responses;
using StatusesApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;
using StatusesApi.Services;
using StatusesApi.Models.View;


[ApiController]
public class GraphsController : ControllerBase
{
    private readonly IStatusService _statusService;
    private readonly StatusDbContext _dbContext;

    public GraphsController(IStatusService statusService, StatusDbContext dbContext)
    {
        _statusService = statusService;
        _dbContext = dbContext;
    }

    private void CheckGraph(CreateGraphRequest request)
    {
        var allVertexes = request.Vertexes;
        foreach (var edge in request.Edges) {
            if (!allVertexes.Contains(edge.From)) {
                allVertexes.Add(edge.From);
            }
            if (!allVertexes.Contains(edge.To)) {
                allVertexes.Add(edge.To);
            }
        }
        
        var unexistingVertexes = allVertexes.Where(v => !_dbContext.Statuses.Any(s => s.Id == v)).ToList();
        if (unexistingVertexes.Count > 0) {
            throw new InvalidGraphException("Граф содержит несуществующие вершины");
        }
        
        // Базовые проверки
        if (request.Vertexes == null || request.Vertexes.Count == 0 || 
            request.Edges == null || request.Edges.Count == 0)
        {
            throw new InvalidGraphException("Некорректная структура графа");
        }

        var vertices = request.Vertexes;
        var edges = request.Edges;

        // 1. Проверка на кратные ребра
        var uniqueEdges = new HashSet<(Guid, Guid)>();
        foreach (var edge in edges)
        {
            if (edge.From == edge.To) {
                throw new InvalidGraphException("Граф содержит петли");
            }
            var pair = (edge.From, edge.To);
            if (uniqueEdges.Contains(pair))
            {
                throw new InvalidGraphException("Граф содержит кратные ребра");
            }
            uniqueEdges.Add(pair);
        }

        // 2. Построение графа смежности
        var graph = new Dictionary<Guid, List<Guid>>();
        foreach (var vertex in vertices)
        {
            graph[vertex] = new List<Guid>();
        }
        
        foreach (var edge in edges)
        {
            if (!graph.ContainsKey(edge.From) || !graph.ContainsKey(edge.To))
            {
                throw new InvalidGraphException("Граф содержит недопустимые вершины");
            }
            graph[edge.From].Add(edge.To);
        }

        // 3. Проверка достижимости из первой вершины
        var visited = new HashSet<Guid>();
        var queue = new Queue<Guid>();
        var start = vertices[0];
        var end = vertices[^1]; // Последняя вершина

        queue.Enqueue(start);
        visited.Add(start);

        bool endReachable = false;
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            
            if (current == end)
            {
                endReachable = true;
            }

            foreach (var neighbor in graph[current])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        // 4. Проверка что все вершины достижимы
        bool allReachable = visited.Count == vertices.Count;

        if (!endReachable || !allReachable) {
            throw new InvalidGraphException("Граф не является связным");
        }
    }

    /// <summary>
    /// Создание графа
    /// </summary>
    /// <param name="request">Запрос на создание графа</param>
    /// <returns>ID созданного графа</returns>
    [HttpPost("api/v1/graphs")]
    [ProducesResponseType(typeof(CreateGraphResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult CreateGraph([FromBody] CreateGraphRequest request)
    {
        try 
        {
            CheckGraph(request);
        } 
        catch (InvalidGraphException e) 
        {
            return BadRequest(new ErrorResponse { Message = e.Message });
        }

        var graph = new Graph
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        var statusFlows = new List<StatusFlow>();
        var statusOrderMap = new Dictionary<Guid, int>();
        
        for (int i = 0; i < request.Vertexes.Count; i++)
        {
            statusOrderMap[request.Vertexes[i]] = i;
        }

        foreach (var vertex in request.Vertexes)
        {
            var nextStatusIds = request.Edges
                .Where(e => e.From == vertex)
                .Select(e => e.To)
                .ToList();

            var statusFlow = new StatusFlow
            {
                GraphId = graph.Id,
                StatusId = vertex,
                OrderNum = statusOrderMap[vertex],
                NextStatusIds = nextStatusIds
            };

            statusFlows.Add(statusFlow);
        }

        _dbContext.Graphs.Add(graph);
        _dbContext.StatusFlows.AddRange(statusFlows);
        _dbContext.SaveChanges();

        return Ok(new CreateGraphResponse
        { 
            GraphId = graph.Id.ToString() 
        });
    }
    
    /// <summary>
    /// Получение доступных переходов для статуса
    /// </summary>
    /// <param name="statusId">ID статуса</param>
    /// <param name="graphId">ID графа</param>
    /// <returns>Список доступных переходов</returns>
    [HttpGet("internal/v1/statuses/transition")]
    public IActionResult GetStatusTransition([FromQuery] Guid? statusId, [FromQuery] Guid graphId)
    {
        var graph = _dbContext.Graphs
            .Include(g => g.StatusFlows)
            .FirstOrDefault(g => g.Id == graphId);
        
        if (graph == null)
        {
            return NotFound(new ErrorResponse { Message = "Граф не найден" });
        }

        var currentStatusFlow = graph.StatusFlows
            .FirstOrDefault(sf => (statusId == null && sf.OrderNum == 0) || (statusId != null && sf.StatusId == statusId));
        
        if (currentStatusFlow == null)
        {
            return NotFound(new ErrorResponse { Message = "Статус не найден в графе" });
        }

        // Получаем доступные переходы
        var nextStatusIds = currentStatusFlow.NextStatusIds;
        var nextStatuses = _dbContext.Statuses
            .Where(s => nextStatusIds.Contains(s.Id))
            .Select(s => new Transition 
            { 
                StatusId = s.Id,
                Name = s.Name
            })
            .ToList();

        return Ok(new TransitionsResponse
        {
            Transitions = nextStatuses
        });
    }

    private List<StatusInGraphResponse> GetStatuses(Guid graphId)
    {
        return _statusService.GetStatusesInGraph(graphId).Select(s => new StatusInGraphResponse(s)).ToList();
    }
    
    /// <summary>
    /// Получение списка графов
    /// </summary>
    /// <param name="graphIds">Список ID графов</param>
    /// <returns>Список графов</returns>
    [HttpGet("api/v1/graphs")]
    [ProducesResponseType(typeof(GetGraphsResponse), StatusCodes.Status200OK)]
    public IActionResult GetGraphs([FromQuery] List<Guid> graphIds)
    {
        var graphs = _dbContext.Graphs
            .Include(g => g.StatusFlows)
            .Where(g => !graphIds.Any() || graphIds.Contains(g.Id))
            .ToList();

        List<GraphResponse> responseGraphs =  graphs.Select(g => new GraphResponse
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Statuses = GetStatuses(g.Id)
            }).ToList();

        return Ok(new GetGraphsResponse {
            Graphs = responseGraphs
        });
        
    }
}