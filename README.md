# Bot.ChatbotDialogGenerator
A console application that helps you create dialogues to an existing application based on Bot Framework V3

# Input
Chatbot generator accepts `.xlsx` as input files. You will find sample inputs in the `\inputs\samples` folder. Mind that some of them contain multiple sheets.

## Basic concepts 
+ The input file structure is similar to a tree. 
+ The root of the tree represents the name of the dialog.
+ All the configuration mentioned below can be set in `\ChatbotDialogGenerator\ChatbotDialogGenerator\App.config` file.

#### File structure
+ You define the dialog name in the first column of the flow. You can configure the first column number by setting `ColumnWithDialogs`. See `\inputs\samples\basic_structure_sample.xlsx` for reference.
+ You start the actual flow in the column determined by `FlowStartColumn` setting.

#### Dialog flow
+ Each dialog flow consists of one or more steps.
+ There are several types of steps:
    + **yes/no** step
    + **choice** step
    + **redirect** step
    + **done** step
+ **yes/no** and **choice** steps consist of three columns - question, options, next step root.
You define options in **choice** step by putting them under the question and separating with new line (`Alt + Enter`). Each choice **must** have a leading digit. See `\inputs\samples\choice_sample.xlsx` for reference.
Step will be considered as **yes/no** if one of its options is *yes* or *no*. See `\inputs\samples\yes_no_sample.xlsx` for reference.
+ **redirect** and **done** steps consist of one column only which represents bot message or redirection target.
Step will be considered as **redirect** if a cell in the first column matches the pattern defined in `RedirectCellPattern`. See `\inputs\samples\redirect_sample.xlsx` for reference. You can use inside name of the dialog to which flow will be redirected or (**if it's inside same dialog**) you can simply put name of the cell i.e {D1} in that case method will be reused. For usage please check Test 1 dialog from `\inputs\sample.xlsx`
+ You can extract repeatable or referenced parts of dialog flow into the separate flow. You do this by creating a new flow below and referencing it with **redirect** step syntax. See `\inputs\samples\redirect_to_subdialog_sample.xlsx` for reference.
+ You can add additional messages to be sent by bot before asking a question. You do this by simply putting some text before question, in the very same cell. Additional messages are separated with new line. Remeber to put a new line between last message and the question. See `\inputs\samples\additional_messages_sample.xlsx` for reference

# Configuration
All the configurations mentioned below can be set in `\ChatbotDialogGenerator\ChatbotDialogGenerator\App.config`

## UnitTests
+ `UnitTestsProjectName`(required) - a name of the test project containing bot dialogs' tests.
+ `UnitTestsFolderPath`(required) - a path to the folder containing dialogs' tests. Relative to test project path (ex. `Dialogs\Generated`).
+ `UnitTestNamespace`(required) - a name of the namespace where all the generated tests will be put.
+ `UnitTestsUsings`(required) - list of namespaces to be included in all the generated dialogs' tests.
+ `UnitTestBaseClass`(required) - a name of a base type for dialogs' tests.
+ `UnitTestDonePredicate`(required) - `context.Done` verification predicate. (ex. `result =&gt; result.DialogResultType == DialogResultType.AskForAnotherQuestion`). Remember about escaping characters.

## CommonSettings
+ `SolutionPath`(required) - absolute path to the folder containing solution file (ex. + `C:\Users\test\src`).
+ `DialogProjectName`(required) - a name of the project containing bot dialogs.
+ `DialogFolderPath`(required) - a path to the folder containing dialogs. Relative to project path (ex. `Dialogs\Generated`).
+ `DialogNamespaceName`(required) - a name of the namespace where all the generated dialogs will be put.
+ `DialogUsings` - list of namespaces to be included in all the generated dialogs.
+ `UsePostAsSeparateBubble`(required) - setting this to true results in generating squashed `context.PostAsSeperateBubblesAsync` instead of multiple `context.PostAsync` invokations. Use only if `context.PostAsSeperateBubblesAsync` is predefined in the dialog project.
+ `BaseDialogInterface` - a name of a base type for dialog interfaces (eg. `IRootDialog`).
+ `InterfaceUsings` - list of namespaces to be included in all the generated dialogs' interfaces.
+ `ExcelFilePath`(required) - absolute path to the input .xlsx file (ex. `c:\Users\test\sample.xlsx`).
+ `InterfaceNamespaceName`(required) - a name of the namespace where all the generated dialog interfaces will be put.
+ `InterfaceProjectName`(required) - a name of the project containing bot dialogs' interfaces.
+ `InterfaceFolderPath`(required) - a path to the folder containing dialogs' interfaces. Relative to interfaces project path (ex. `Dialogs\Generated`).
+ `ResultObject`  - dialog result type.
+ `ResourcesFileName` - name of the resources file (ex. `Resources.resx`).
+ `SolutionName` - name of the solution.
+ `ResourceAccessorUsings` - list of namespaces to be included in all resources accessors.
+ `TicketDialog`(required) - name of a dialog to be redirected to when using `TicketCreationText`.

## ExcelSettings
+ `RedirectCellPattern`(required) - regular expression pattern for redirect cells (ex. `\{(\D\d)\}`).
+ `ColumnWithDialogs`(greater than 0) - number of column where dialog definition starts. Cells in this column contain dialogs names. (ex. `1` is an equivalent of `A` column).
+ `FlowStartColumn`(greater than 0) - number of column where dialog flow starts (first step of the dialog).
+ `ChoiceSeperator` - used when there are multiple choices groupped into one (ex. when you want to group steps 1,2,3 into one you can put `1&2&3` into the cell and set this setting to `&`).
+ `DefaultEndAction`(required) - default `context.Done()` argument.
+ `TicketCreationText`(required) - ticket dialog selector. (ex. `[ticket creation]`).
+ `DefaultEndText`(required) - text displayed at flow's end.