using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi;

[assembly: OwinStartup(typeof(HamburgersBlog.Startup))]

namespace HamburgersBlog
{
    public class Startup
    {
        private const string TWITTER_CONSUMER_ID = "pv8F32sC7mOIpYwt8Mbx4bHc4";
        private const string TWITTER_CONSUMER_SECRET = "BS3hZpBsDg0aoRTWMTu5fWDrGEl6XStEMb0BdIW482rbZj08Qk";
        private const string TWITTER_ACCESS_TOKEN = "985450562434068482-QnmHOcBhCr6MJ5nEZYkgVeo5lix9OA3";
        private const string TWITTER_ACCESS_TOKEN_SECRET = "scIBhLzShg2GVKxyibcz8dScM8wiF0F4jKGn09LBRqAry";

        public void Configuration(IAppBuilder app)
        {
            Auth.SetUserCredentials(TWITTER_CONSUMER_ID,
                                    TWITTER_CONSUMER_SECRET,
                                    TWITTER_ACCESS_TOKEN,
                                    TWITTER_ACCESS_TOKEN_SECRET);
        }
    }
}