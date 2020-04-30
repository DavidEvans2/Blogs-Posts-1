using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;

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
                    Console.WriteLine("1) Display your list of Blogs");
                    Console.WriteLine("2) Make a Blog");
                    Console.WriteLine("3) Make a Post");
                    Console.WriteLine("4) Display your Posts");
                    Console.WriteLine("Press any key to quit");

                    userChoice = Console.ReadLine();
                    logger.Info("User choice: ", userChoice);


                    if (userChoice == "1")
                    {
                        // Display all Blogs from the database
                        var database = new BloggingContext();
                        var sqlQuery = database.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine($"{sqlQuery.Count()} Blogs");
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
                        if (name.Length == 0)
                        {
                            logger.Error("Please enter a name for your Blog, it can't be blank.");
                        }
                        else
                        {
                            var blog = new Blog { Name = name };

                            var database = new BloggingContext();
                            database.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                        }
                    }


                    else if (userChoice == "3")
                    {
                        var database = new BloggingContext();              
                        var sqlQuery = database.Blogs.OrderBy(b => b.BlogId);
                        
                        Console.WriteLine("Which blog do you want to post to? : ");
                        foreach (var item in sqlQuery)
                        {
                            Console.WriteLine($"{item.BlogId}) {item.Name}");
                        }
                        
                        if(int.TryParse(Console.ReadLine(), out int BlogID))
                        {
                            if(database.Blogs.Any(b => b.BlogId == BlogID))
                            {
                                Post post = new Post();
                                post.BlogId = BlogID;
                                Console.WriteLine("Name the title of your Post:");
                                post.Title = Console.ReadLine(); 
                                logger.Info("Post added - {postTitle}", post.Title);
                                if(post.Title.Length == 0)
                                {
                                    logger.Info("Sorry partner, your title can't be blank");
                                }
                                else
                                {
                                    Console.WriteLine("Wanna add some content to your post?");
                                   post.Content = Console.ReadLine(); 
                                   database.AddPost(post);
                                   logger.Info("Content added - {Content}", post.Content);
                                }
                            }
                            else
                            {
                                logger.Error("Hmmm, seems there aren't any blogs with that ID Number");
                            }
                        } 
                        else
                        {
                            logger.Error("Invalid Blog ID Number");
                        }                                                                    
                    }

                    else if (userChoice == "4")
                    {
                        var database = new BloggingContext();
                        var sqlQuery = database.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("Which Blog do you want to see posts from?");

                        foreach(var item in sqlQuery)
                        {
                            Console.WriteLine($"{item.BlogId}) {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            IEnumerable<Post> Posts;
                            if (BlogId != 0 && database.Blogs.Count(b => b.BlogId == BlogId) == 0)
                            {
                                logger.Error("Hmmm, there aren't any blogs saved with that ID Number");
                            }
                            else
                            {
                                
                                Posts = database.Posts.OrderBy(p => p.Title);
                                if (BlogId == 0)
                                {
                                   
                                    Posts = database.Posts.OrderBy(p => p.Title);
                                }
                                else
                                {
                                   
                                    Posts = database.Posts.Where(p => p.BlogId == BlogId).OrderBy(p => p.Title);
                                }
                                Console.WriteLine($"{Posts.Count()} post/s returned");
                                foreach (var item in Posts)
                                {
                                    Console.WriteLine($"Blog: {item.Blog.Name}\nTitle: {item.Title}\nContent: {item.Content}\n");
                                }
                            }
                        }
                        else
                        {
                            logger.Error("Error! Blog ID Number nonexistent!");
                        }
                    }
                } while (userChoice == "1" || userChoice == "2" || userChoice == "3" || userChoice == "4");
                logger.Info("Program has ended");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}