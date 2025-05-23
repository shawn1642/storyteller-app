### Task 3 – Fix Importance Mapping (Section 1)

- Issue:The Importance value was not preserved when converting a TodoItem to TodoItemEditFields, causing the test EqualImportance to fail.

- Diagnosis:The test setup correctly passed Importance.High using a test builder. However, in the TodoItemEditFields constructor, the line Importance = Importance.Medium ignored the incoming parameter and always defaulted the value.

- Solution:Replaced the hardcoded line with:

    - Importance = importance;

- This allows the constructor to honor the input value passed from the mapping factory.

- Result:All tests in WhenTodoItemIsConvertedToEditFields now pass, confirming the mapping is correct.

- Commit Reference:feature/fix-importance-mapping — commit message: fix: correctly map Importance when converting TodoItem to TodoItemEditFields

### (Section 2: UI Enhancements and Feature Fixes)

### Task 4: Update Views/TodoList/Detail.cshtml to display items sorted by importance
## Approach and Logic

-   Although the instruction refers to modifying the Razor view directly, a cleaner and more maintainable approach was to move the sorting logic into the ViewModel factory (TodoListDetailViewmodelFactory). This aligns with the Single Responsibility Principle by keeping the Razor view focused on rendering, while the factory handles data shaping.

-   The following change was made in TodoListDetailViewmodelFactory.Create(...):
    var items = todoList.Items
        .OrderByDescending(x => x.Importance)
        .Select(TodoItemSummaryViewmodelFactory.Create)
        .ToList();

- This ensures that items are sorted in the correct order (High > Medium > Low) before they reach the view, resulting in cleaner Razor code and better separation of concerns.

### Task 5 - Replace "ResponsiblePartyId" with friendly display text

- Files Updated:
Views/TodoItem/Create.cshtml
Views/TodoItem/Edit.cshtml

# Issue:

- The label for assigning a responsible user was displayed as the raw property name "ResponsiblePartyId", which may be unclear or confusing for users.

# Solution:

- Replaced the default Razor label binding with a more user-friendly label:
    <label asp-for="ResponsiblePartyId">Assigned To</label>

This change keeps the asp-for directive to preserve form validation and model binding, while improving the label text for clarity.

# Logic:

- Enhancing field labels with user-friendly text improves the overall user experience, especially for non-technical users. The change maintains full functionality while aligning the terminology with user expectations.

### Task 6 – Add ability to toggle visibility of completed items

# Files Updated:
TodoListController.cs
Views/TodoList/Detail.cshtml

# Issue:

- The UI displayed both completed and pending items without giving the user control to filter out completed tasks.

# Solution:

- Added a hideCompleted query parameter to the Detail action in TodoListController. If hideCompleted is true, the controller filters out completed items before passing them to the ViewModel factory.

- In the view (Detail.cshtml), added a toggle button that updates the URL with hideCompleted=true or false, allowing users to switch between viewing all items and hiding completed ones.

# Logic:

This approach keeps the filtering logic in the controller, avoids overloading the view, and maintains a stateless, link-driven UI using query parameters. The solution improves usability without complicating the user experience or data model.

### Task 6 – Show Relevant Todo Lists

# Approach:
- To meet the requirement of showing all Todo lists where the current user is either the owner or assigned as the responsible party for at least one item:

- A new method RelevantTodoLists(userId) was added to ApplicationDbContext.
- It filters the lists using an Include(...).ThenInclude(...) chain to ensure items and their related ResponsibleParty are loaded.
- The query returns lists where:
    tl.Owner.Id == userId (the user owns the list), or
    tl.Items.Any(i => i.ResponsiblePartyId == userId) (the user is assigned to at least one item).
    The TodoListController.Index() method uses this new query.

### (Section 3: Advanced enhancements and APIs)

### Task 8: Add Rank Field to TodoItem
# Files Updated:
Data/Entities/TodoItem.cs
Models/TodoItems/TodoItemEditFields.cs
Models/TodoItems/TodoItemSummaryViewmodel.cs
EntityModelMappers/TodoItems/TodoItemEditFieldsFactory.cs
EntityModelMappers/TodoItems/TodoItemSummaryViewmodelFactory.cs
Views/TodoItem/Create.cshtml
Views/TodoItem/Edit.cshtml

# Issue:
- The TodoItem model lacked a Rank field to represent ordering or priority beyond importance.

# Solution:

- Added Rank to the TodoItem entity.
- Updated related viewmodels and their factories to support mapping.
- Added inputs to both the Create and Edit forms.
- Created and applied an EF Core migration to add the column to the database.

# Logic:
The update was done following the MVC architecture, ensuring separation of concerns. Form input is properly bound, validated, and persisted. This would set up potential ordering or custom list views for future improvements.

### Task 9: Enhance User Identification with Gravatar
# Goal
Display a user’s name alongside their email and profile image using the Gravatar API, with graceful fallback in case Gravatar is unavailable.

# Approach
User Information Formatting
The view was updated to display the user's UserName followed by their Email:
@item.ResponsibleParty.UserName (@item.ResponsibleParty.Email)

# Gravatar Integration
The user's email is hashed and passed to the Gravatar API to retrieve a profile image:
<img src="https://www.gravatar.com/avatar/@Gravatar.GetHash(item.ResponsibleParty.Email)?s=30&d=mp"
     alt="User avatar"
     onerror="this.onerror=null;this.src='https://www.gravatar.com/avatar/?d=mp&s=30';" />

# Fallback Handling
The onerror attribute ensures that if the Gravatar service fails or the image is not found, a default image is shown (d=mp).

# Impact
The solution improves user identification clarity in the UI while preventing broken image links and maintaining a clean, professional display.