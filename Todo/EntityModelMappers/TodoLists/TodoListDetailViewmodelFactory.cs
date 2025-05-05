using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList)
        {
            var items = todoList.Items
                .Select(TodoItemSummaryViewmodelFactory.Create)
                .OrderBy(i => i.Importance == Importance.High ? 0 : i.Importance == Importance.Medium ? 1 : 2)
                .ToList();

            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items);
        }
    }
}