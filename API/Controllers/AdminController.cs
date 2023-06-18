using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.DTOs;
using API.DTOs.Admin;
using API.Helpers;
using API.Interfaces;
using API.Models;
using API.Models.OrderAggregate;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        public AdminController(UserManager<AppUser> userManager, IMapper mapper, IOrderService orderService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRolesAsync(int pageIndex, int pageSize)
        {
            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Photo)
                .Include(u => u.Orders)
                .Include(u => u.Addresses)
                .OrderBy(u => u.Id)
                .Skip(1 + (pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<UserForAdminDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            var totalUsers = await _userManager.Users.CountAsync();

            return Ok(new Pagination<UserForAdminDto>(pageIndex, pageSize, totalUsers, users));
        }

        [HttpGet("users-with-roles/{id}")]
        public async Task<ActionResult> GetUserWithRolesByIdAsync(int id)
        {
            var user = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Photo)
                .Include(u => u.Addresses)
                .Include(u => u.Orders)
                .Where(u => u.Id == id)
                .ProjectTo<UserForAdminDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return Ok(user);
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IReadOnlyCollection<OrderToReturnDto>>> GetOrdersAsync([FromQuery] OrderSpecParams orderParams)
        {            
            var orders = await _orderService.GetOrdersForAdminAsync(orderParams);

            var totalOrders = await _orderService.GetOrdersCountAsync(orderParams);

            return Ok(new Pagination<OrderForAdminDto>(orderParams.PageIndex, orderParams.PageSize, totalOrders, _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderForAdminDto>>(orders)));
        }

        [HttpGet("orders/{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdAsync(int id)
        {
            var order = await _orderService.GetOrderForAdminByIdAsync(id);

            if (order == null) return NotFound();

            return Ok(_mapper.Map<Order, OrderForAdminDto>(order));
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not find user..");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add user to roles..");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove user from roles..");
        
            return Ok(await _userManager.GetRolesAsync(user));
        }
        
        // [HttpGet("users-with-photos")]
        // public ActionResult GetPhotosForModeration()
        // {
        //     return Ok("Only admins and moderators can see this");
        // }
    }
}

/*
"  "   at Microsoft.Data.Sqlite.SqliteException.ThrowExceptionForRC(Int32 rc, sqlite3 db)
   at Microsoft.Data.Sqlite.SqliteCommand.PrepareAndEnumerateStatements(Stopwatch timer)+MoveNext()
   at Microsoft.Data.Sqlite.SqliteCommand.GetStatements(Stopwatch timer)+MoveNext()
   at Microsoft.Data.Sqlite.SqliteDataReader.NextResult()
   at Microsoft.Data.Sqlite.SqliteCommand.ExecuteReader(CommandBehavior behavior)
   at Microsoft.Data.Sqlite.SqliteCommand.ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
   at Microsoft.Data.Sqlite.SqliteCommand.ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Query.Internal.SingleQueryingEnumerable`1.AsyncEnumerator.InitializeReaderAsync(DbContext _, Boolean result, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Query.Internal.SingleQueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
   at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
   at API.Controllers.AdminController.GetUsersWithRolesAsync(Int32 pageIndex, Int32 pageSize) in J:\Vs Code projects\RedBeard (under maintain)\api\Controllers\AdminController.cs:line 36
   at lambda_method178(Closure , Object )
   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.TaskOfActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()
--- End of stack trace from previous location ---
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|19_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Logged|17_1(ResourceInvoker invoker)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)
   at Microsoft.AspNetCore.Authorization.Policy.AuthorizationMiddlewareResultHandler.HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
   at API.MIddleware.ExceptionMiddleware.InvokeAsync(HttpContext context) in J:\Vs Code projects\RedBeard (under maintain)\api\MIddleware\ExceptionMiddleware.cs:line 30"
   */