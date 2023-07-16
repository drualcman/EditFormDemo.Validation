# Manual validation EditForm Component Demo
This project it´s a simple example how to validate a model when you don´t have access to the model code.

## Scenario
* The class ```ExampleModelFromExternalDll``` represent a exterdal model we can't modify.
* We will manually validate some properties
* Must be show a validation error messages
* When is validated can send the form

### Example 
Component
``` RAZOR
    <EditForm EditContext=FormContext OnValidSubmit=SubmitForm>
        <div class="field">
            <label class="label">First Name</label>
            <div class="control">
                <InputText @bind-Value=Model.FirstName class="input" />
                <ValidationMessage For="() => Model.FirstName" />
            </div>
        </div>
        <div class="field">
            <label class="label">Email</label>
            <div class="control">
                <InputText @bind-Value=Model.Email class="input" type="email" />
                <ValidationMessage For="() => Model.Email" />
            </div>
        </div>    
        <div class="field">
            <div class="control">
                <button class="button is-success">Submit</button>
            </div>
            <label class="label">@Messages</label>
        </div>
    </EditForm>
```
Code
``` CSHARP
string Messages;
    ExampleModelFromExternalDll Model = new();
    EditContext FormContext;
    ValidationMessageStore ValidationMessageStore;

    protected override void OnInitialized() 
    {
        FormContext = new EditContext(Model);
        FormContext.OnFieldChanged += FormContext_OnFieldChanged;
        FormContext.OnValidationRequested += FormContext_OnValidationRequested;
        ValidationMessageStore = new ValidationMessageStore(FormContext);
    }

    private void FormContext_OnValidationRequested(object sender, ValidationRequestedEventArgs e) 
    {
        Messages = "";
        ValidationMessageStore.Clear();        
        if(string.IsNullOrWhiteSpace(Model.FirstName))
            ValidationMessageStore.Add(new FieldIdentifier(Model, nameof(Model.FirstName)), "First name required");
        if(string.IsNullOrWhiteSpace(Model.Email))
            ValidationMessageStore.Add(new FieldIdentifier(Model, nameof(Model.Email)), "Email required");
        FormContext.NotifyValidationStateChanged();
    }

    private void FormContext_OnFieldChanged(object sender, FieldChangedEventArgs e) 
    {      
        Messages = "";
        ValidationMessageStore.Clear();
        switch(e.FieldIdentifier.FieldName)
        {
            case nameof(Model.FirstName):
                if(string.IsNullOrWhiteSpace(Model.FirstName))
                    ValidationMessageStore.Add(e.FieldIdentifier, "First name required");
                break;
            case nameof(Model.Email):
                if(string.IsNullOrWhiteSpace(Model.Email))
                    ValidationMessageStore.Add(e.FieldIdentifier, "Email required");  
                if(!Model.Email.Contains("@"))
                    ValidationMessageStore.Add(e.FieldIdentifier, "Email not valid");
                break;
        }
        FormContext.NotifyValidationStateChanged();
    }

    void SubmitForm()
    {
        Messages = "Form will be send!";
    }
```
