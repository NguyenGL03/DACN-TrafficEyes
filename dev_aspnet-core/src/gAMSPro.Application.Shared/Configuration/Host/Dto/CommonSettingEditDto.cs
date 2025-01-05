namespace gAMSPro.Configuration.Host.Dto
{
    public class CommonSettingEditDto
    {
        public string DefaultRecordsCountPerPage { get; set; }
        public string ResizableColumns { get; set; }
        public string IsResponsive { get; set; }
        public string PredefinedRecordsCountPerPage { get; set; }
        public string PhoneNumberRegexValidation { get; set; }
        public string DateTimeFormatClient { get; set; }
        public string DatePickerDisplayFormat { get; set; }
        public string DatePickerValueFormat { get; set; }
        public bool? LanguageComboboxEnable { get; set; }
        public int? TimeShowSuccessMessage { get; set; }
        public int? TimeShowWarningMessage { get; set; }
        public int? TimeShowErrorMessage { get; set; }
        public string EmailRegexValidation { get; set; }
        public string CoreNoteRegexValidation { get; set; }
        public string CodeNumberRegexValidation { get; set; }
        public string NumberPlateRegexValidation { get; set; }
        public int? MaxQuantityNumber { get; set; }
        public string TaxNoRegexValidation { get; set; }
        public string FullNameRegexValidation { get; set; }
        public string MaxLenghtRegexValidation { get; set; } 
        public int? FileSizeAttach { get; set; }
        public string FileExtensionAttach { get; set; }
    }
}