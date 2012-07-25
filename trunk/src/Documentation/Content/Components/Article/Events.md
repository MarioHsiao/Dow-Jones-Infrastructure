Article component raises the following events:

Event							| Description																											
--------------------------------|-------------------------------------------------------------------------------------------
entityClick.dj.Article			| Raised when an entity is clicked. Passes Entity object as the argument.
sourceClick.dj.Article			| Raised when article source is clicked. Passes Entity object as the argument.
authorClick.dj.Article			| Raised when article author is clicked. Passes Entity object as the argument.
anchorClick.dj.Article			| Raised when an anchor is clicked. Passes Entity object as the argument.
postprocessing.dj.article		| Raised when a post-processing button is clicked. Passes Headline object with button type as the argument.
eLinkClick.dj.Article			| Raised when a link inside the article is clicked. Passes Entity object as the argument.
headlineLinkClick.dj.article	| Raised when article headline is clicked. Passes Headline object as the argument.
smallPictureClick.dj.article	| Raised when a picture in the article is clicked. Passes the large image source url as the argument.
enlargeImageLinkClick.dj.Article| Raised when enlarge image link is clicked. Passes the large image source url as the argument.
