
//Ma: using Mock feed data an templating functions.
self = "{ tweets: mockTweets.data, options: { maxTweetsToShow: 20} }";

function anonymous(self) {

 var out = '<div class="stream-items">    ';
 var tweetId, userId, screenName, reTweetId, fullName, profileUrl, profileHashUrl, profileImageUrl, tweetText, 
                        tweetTimeStamp, tweetTimeStampRaw, 
                        tweets = self.tweets, options = self.options;
 for (var i = 0, len = Math.min(tweets.length, options.maxTweetsToShow);i < len;i++) { 
 t = tweets[i];
 tweetId = t.id;
 tweetText = t.text;
 reTweetId = t.reTweetId;
 userId = t.user.id;
 screenName = t.user.screen_name;
 fullName = t.user.name;
 profileImageUrl = t.user.profile_image_url;
 profileUrl = 'http://twitter.com/' + screenName;
 profileHashUrl = '/#!/' + screenName;
 statusHashUrl = profileHashUrl + '/status/' + screenName;
 out += '    <div data-tweet-id="' + (tweetId) + '" class="stream-item">        <div data-user-id="' + (userId) + '" data-screen-name="' + (screenName) + '" data-retweet-id="' + (reTweetId) + '"            data-tweet-id="' + (tweetId) + '" class="stream-item-content tweet stream-tweet ">            <div class="tweet-dogear ">            </div>            <div class="tweet-image">                <img width="48" height="48" data-user-id="' + (userId) + '" class="user-profile-link"                    alt="' + (fullName) + '" src="' + (profileImageUrl) + '">            </div>            <div class="tweet-content">                <div class="tweet-row">                    <span class="tweet-user-name"><a title="' + (fullName) + '" href="' + (profileHashUrl) + '"                        data-user-id="' + (userId) + '" class="tweet-screen-name user-profile-link">                        ' + (screenName) + '</a> <span class="tweet-full-name">                            ' + (fullName) + '</span> </span>                    <div class="tweet-corner">                        <div class="tweet-meta">                            <span class="icons"><span class="retweet-icon"></span><em>by                                ' + (screenName) + '</em><div class="extra-icons">                                    <span class="reply-icon icon">@</span> <span class="inlinemedia-icons"></span>                                </div>                            </span>                        </div>                    </div>                </div>                <div class="tweet-row">                    <div class="tweet-text pretty-link">                        <a rel="nofollow" href="' + (profileUrl) + '" data-screen-name="' + (screenName) + '"                            class="  twitter-atreply"><span class="at">@</span><span class="at-text">' + (screenName) + '</span></a>                        ' + (tweetText) + '</div>                </div>                <div class="tweet-row">                    <a title="' + (tweetTimeStamp) + '" class="tweet-timestamp" href="' + (statusHashUrl) + '">                        <span data-long-form="true" data-time="' + (tweetTimeStampRaw) + '" class="_timestamp">                            ' + (userId) + '</span></a> <span data-tweet-id="' + (tweetId) + '" class="tweet-actions">                                <span class="tweet-action action-favorite"><a title="Favorite" class="favorite-action"                                    href="#"><span><i></i><b>Favorite</b></span></a> <span class="activity-count favoriter-count">                                    </span></span><span class="tweet-action action-retweet"><a title="Retweet" class="retweet-action"                                        href="#"><span><i></i><b>Retweet</b></span></a> <span class="activity-count retweeter-count">                                        </span></span><a title="Reply" data-screen-name="' + (screenName) + '" class="reply-action"                                            href="#"><span><i></i><b>Reply</b></span></a> </span>                </div>            </div>        </div>    </div>    ';
 } out += '</div>';
 return out;

}