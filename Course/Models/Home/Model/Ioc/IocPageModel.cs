namespace Course.Models.Home.Model.Ioc
{
    public class IocPageModel
    {
        public String Title { get; set; } = null!;
        public String SingleHash { get; set; } = null!;
        public Dictionary<String, String> Hashes { get; set; } = new();
		public string TabHeader { get; internal set; }
		public string PageTitle { get; internal set; }
		public string IoCIs { get; internal set; }
		public List<string> IoCOptions { get; internal set; }
		public List<string> HashExm { get; internal set; }
	}
}
