using HamburgersBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HamburgersBlog.DAL
{
    public class ProjectInitializer : System.Data.Entity.DropCreateDatabaseAlways<ProjectContext>
    {
        protected override void Seed(ProjectContext context)
        {
            var princesses = new List<Princess>
            {
                new Princess
                {
                    PrincessID=1,
                    Name="Pocahontas",
                    RoyaltyType=KingdomType.RoyalBlood,
                    CreationYear=1995,
                    HairColor=HairColor.Black,
                    Nationality="Northern Territory, Australia",
                    MovieName="Pocahontas"
                },
                new Princess
                {
                    PrincessID=2,
                    Name="Snow White",
                    RoyaltyType=KingdomType.RoyalBlood,
                    CreationYear=1937,
                    HairColor=HairColor.Black,
                    Nationality="Germany",
                    MovieName="Snow White and the Seven Dwarfs"
                },
                new Princess
                {
                    PrincessID=3,
                    Name="Cinderella",
                    RoyaltyType=KingdomType.MarriedToRoyalty,
                    CreationYear=1950,
                    HairColor=HairColor.Yellow,
                    Nationality="France",
                    MovieName="Cinderella"
                },
                 new Princess
                {
                    PrincessID=4,
                    Name="Mulan",
                    RoyaltyType=KingdomType.NotRoyal,
                    CreationYear=1998,
                    HairColor=HairColor.Black,
                    Nationality="China",
                    MovieName="Mulan"
                },
                 new Princess
                {
                    PrincessID=5,
                    Name="Ariel",
                    RoyaltyType=KingdomType.RoyalBlood,
                    CreationYear=1989,
                    HairColor=HairColor.Red,
                    Nationality="Atlantic Ocean",
                    MovieName="The Little Mermaid"
                },
                 new Princess
                {
                    PrincessID=6,
                    Name="Esmeralda",
                    RoyaltyType=KingdomType.NotRoyal,
                    CreationYear=1996,
                    HairColor=HairColor.Black,
                    Nationality="Paris, France",
                    MovieName="The Hunchback of Notre Dame"
                },
                 new Princess
                {
                    PrincessID=7,
                    Name="Elsa",
                    RoyaltyType=KingdomType.RoyalBlood,
                    CreationYear=2013,
                    HairColor=HairColor.Yellow,
                    Nationality="Norway",
                    MovieName="Frozen"
                },
                 new Princess
                {
                    PrincessID=8,
                    Name="Meg",
                    RoyaltyType=KingdomType.RoyalBlood,
                    CreationYear=1997,
                    HairColor=HairColor.Red,
                    Nationality="Greece",
                    MovieName="Hercules"
                },
                 new Princess
                {
                    PrincessID=9,
                    Name="Jane",
                    RoyaltyType=KingdomType.NotRoyal,
                    CreationYear=1999,
                    HairColor=HairColor.Red,
                    Nationality="Africa",
                    MovieName="Tarzan"
                },
            };
            princesses.ForEach(h => context.Princesses.Add(h));
            context.SaveChanges();

            var posts = new List<Post>
            {
                new Post {
                    PostID = 1,
                    Title = "Why Moana Is The Best Disney Princess",
                    AuthorName = "Shahar Hacohen",
                    Date=new DateTime(2018, 5, 9),
                    Content="She is a strong, independent woman who don't need no man",
                    PrincessID=princesses[0].PrincessID,
                },
                new Post  {
                    PostID = 2,
                    Title = "I Wish I Was a Disney Princess",
                    AuthorName="Shani Hollander",
                    Date=new DateTime(2018,5,2),
                    Content="I wish I could be a Disney princess so I could have furniture as my only friends",
                    PrincessID=princesses[1].PrincessID,
                },
                new Post  {
                    PostID = 3,
                    Title = "The Truth About Snow White",
                    AuthorName="Liza Gulitski",
                    Date=new DateTime(2018,4,30),
                    Content="She got no swag",
                    PrincessID=princesses[1].PrincessID,
                },
                new Post  {
                    PostID = 4,
                    Title = "The Sleeping Beauty Conspiracy",
                    AuthorName="Guest Author",
                    Date=new DateTime(2018,4,26),
                    Content="She was awake the whole time",
                    PrincessID=princesses[0].PrincessID,
                },
                new Post  {
                    PostID = 5,
                    Title = "What Disney Didn't Want You To Know",
                    AuthorName="Shani Hollander",
                    Date=new DateTime(2018,4,21),
                    Content="Snow white is actually a dude",
                    PrincessID=princesses[2].PrincessID,
                }
            };

            posts.ForEach(p => context.Posts.Add(p));
            context.SaveChanges();

            var comments = new List<Comment>
            {
                new Comment
                {
                    CommentID=1,
                    PostID=1,
                    Title="You are right!",
                    AuthorName="MoanaLover123",
                    Content="She is the best!",
                },
                new Comment
                {
                    CommentID=2,
                    PostID=1,
                    Title="You are wrong!",
                    AuthorName="MoanaHater123",
                    Content="She is the worst!",
                },
                new Comment
                {
                    CommentID=3,
                    PostID=2,
                    Title="Ill be your friend, Shani",
                    AuthorName="A Night Stand",
                    Content="Im a furniture, so i have like.. all of the qualifications",
                },
            };
            comments.ForEach(c => context.Comments.Add(c));
            context.SaveChanges();

            var maps = new List<Map>();

            for (int i = 0; i < princesses.ToArray().Length; i++)
            {
                Map map = new Map
                {
                    MapID = princesses[i].PrincessID,
                    Name = princesses[i].Name,
                    Address = princesses[i].Nationality,
                };

                maps.Add(map);
            }
            maps.ForEach(m => context.Maps.Add(m));
            context.SaveChanges();


            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    RestaurantId=1,
                    Location="Tel Aviv",
                    Name="Vitrina",
                    Rate=5.0,
                    IsKosher=false,
                    IsParkingAvailable=false,
                    IsVeganFriendly=true,
                    Area=Area.Hadarom,
                },
                new Restaurant
                {
                    RestaurantId=2,
                    Location="Rishon Lezion",
                    Name="SuSu And Sons",
                    Rate=3.0,
                    IsKosher=true,
                    IsParkingAvailable=true,
                    IsVeganFriendly=false,
                    Area=Area.Hamerkaz,
                },
                new Restaurant
                {
                    RestaurantId=3,
                    Location="Netanya",
                    Name="Humongous",
                    Rate=4.0,
                    IsKosher =true,
                    IsParkingAvailable=true,
                    IsVeganFriendly=true,
                    Area=Area.Hadarom,
                },
            };
            restaurants.ForEach(r => context.Restaurants.Add(r));
            context.SaveChanges();

            var hamburgers = new List<Hamburger>
            {
                new Hamburger
                {
                    HamburgerID=1,
                    Name="Mexican Burger",
                    Description="very spicy",
                    Price=55,
                },
                new Hamburger
                {
                    HamburgerID=2,
                    Name="Butler Burger",
                    Description="Classic burger",
                    Price=57,
                },
                new Hamburger
                {
                    HamburgerID=3,
                    Name="Butler Cheese Burger",
                    Description="Classic burger with cheese and a love :)",
                    Price=60,
                },
            };
            hamburgers.ForEach(h => context.Hamburgers.Add(h));
            context.SaveChanges();

            var sideDishes = new List<SideDish>
            {
                new SideDish
                {
                    SideDishId=1,
                    Name="Chips",
                    Description="a regular chips",
                    Price=12,
                },
                new SideDish
                {
                    SideDishId=2,
                    Name="Sweet Potato Chips",
                    Description="chips that made from sweet potato",
                    Price=15,
                },
                new SideDish
                {
                    SideDishId=3,
                    Name="Rice",
                    Description="white rice",
                    Price=18,
                },
            };

            sideDishes.ForEach(s => context.SideDishes.Add(s));
            context.SaveChanges();


            var reviews = new List<Review>
            {
                new Review
                {
                    ReviewId=1,
                    Title="very tasty",
                    AuthorName="Shlomi",
                    Content="best burger in town",
                    RestaurantId=1,
                },
                new Review
                {
                    ReviewId=2,
                    Title="yum yum",
                    AuthorName="Itzik a gadol",
                    Content="Wow, what a burger!",
                    RestaurantId=2,
                },
                new Review
                {
                    ReviewId=2,
                    Title="Too expensive",
                    AuthorName="Young Itzik",
                    Content="I like the burger but the price is too high!",
                    RestaurantId=3,
                },
            };

            reviews.ForEach(r => context.Reviews.Add(r));
            context.SaveChanges();
        }      
    }
}