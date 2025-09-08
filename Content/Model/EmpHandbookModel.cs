namespace wwrc_maui.Content.Model
{
    public class EmpHandbookModel
    {
        public class EmployeeHandbookMainModel
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public bool IsFile { get; set; }
            public string File { get; set; } = "";
            public string FileImage { get; set; } = "";
            public string FileType { get; set; } = "";
            public string Filesize { get; set; } = "";
            public string CreateByName { get; set; } = "";
            public DateTime CreateDate { get; set; }
            public string IsRead { get; set; } = "";
        }

        public class API_EmployeeHandbook
        {
            public string DBase { get; set; } = "";
            public string BranchId { get; set; } = "";
            public string FolderId { get; set; } = "";
        }
    }
}
