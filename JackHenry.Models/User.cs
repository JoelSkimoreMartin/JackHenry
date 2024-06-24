using System;

namespace JackHenry.Models;

public class User
{
	public string Name { get; set; }
	public Post[] Posts { get; set; } = Array.Empty<Post>();
}