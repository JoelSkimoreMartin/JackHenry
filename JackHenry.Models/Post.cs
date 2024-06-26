namespace JackHenry.Models;

public class Post
{
	public string Id { get; set; }
	public string Title { get; set; }
	public int UpVotes { get; set; }
	public string Author { get; set; }

	public override string ToString() => Title;
}