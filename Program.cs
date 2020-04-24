using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string userChoice = "";
            try
            {
                do
                {
                    Console.WriteLine("1) Display list of Blogs");
                    Console.WriteLine("2) Make a Blog");
                    Console.WriteLine("3) Make a Post");
                    Console.WriteLine("Press any key to quit");
                   
                    userChoice = Console.ReadLine();
                    logger.Info("User choice: ", userChoice);


                    if (userChoice == "1")
                    {// Display all Blogs from the database
                        var database = new BloggingContext();
                        var sqlQuery = database.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("List of Blogs: ");
                        foreach (var item in sqlQuery)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }

                    else if (userChoice == "2")
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var database = new BloggingContext();
                        database.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    else if (userChoice == "3")
                    {
                        var database = new BloggingContext();
                        Console.WriteLine("Which blog do you want to post to? : ");

                        var blogName = Console.ReadLine();
                        var blog = database.Blogs.FirstOrDefault(b => b.Name.Contains(blogName));

                        Console.WriteLine("Name the title of your Post: ");
                        var postTitle = Console.ReadLine();

                        Console.WriteLine("Wanna add some content to your post?");
                        var postContent = Console.ReadLine();

                        var post = new Post { Title = postTitle, Content = postContent, BlogId = blog.BlogId };
                        database.AddPost(post);
                        logger.Info("Post added - {postTitle}", postTitle);
                        logger.Info("Content added - {postContent}", postContent);
                    }
                } while (userChoice == "1" || userChoice == "2" || userChoice == "3");
                logger.Info("Program has ended");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}