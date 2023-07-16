namespace EditFormDemo.Validation.Pages;

public partial class Index
{
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
}
