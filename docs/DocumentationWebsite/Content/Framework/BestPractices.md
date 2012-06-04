Required Skills
===============

Working with the Dow Jones Web Services requires experience or familiarity with RESTful APIs. Skills involved include .NET, Java, among others. Using Dow Jones Widgets requires experience with HTML, CSS, and JavaScript. To create a custom workflow or a seamless integration experience with the Widgets may require in-depth knowledge of JavaScript.

Implementing Widgets and APIs
=============================
Widgets and APIs can be implemented in a variety of applications, each with their own intricacies as described below.

Intranet Sites
--------------
Integrating Widgets and APIs into Homegrown web products that are internal for use only by the customer’s employees are more likely to require personalization on the Dow Jones and Factiva platform, which may require the use of Session IDs (heavyweight sessions) when authenticating. It is useful to assess whether user-specific personalized assets will be required, to assist in the implementation.

Off-the-shelf Portal Site
-------------------------
Dow Jones APIs or Widgets have not yet been integrated into Sharepoint or Websphere portals; we haven’t yet experienced this implementation, but don’t foresee any complications.

Websites
--------
Web pages with heavy use of script can encounter performance issues. This is not a Widget issue, but rather an issue with how browsers process JavaScript. Modern web browsers can better handle increase use of JavaScript. But as a general rule, too many “controls” on a page, making multiple async requests to external services, can result in a poor user experience.

Due to typical higher volumes usage of a destination website, it is more likely to encounter usage restrictions from the Dow Jones and Factiva platform. Therefore usage volumes assessments are required for proper setting. For example:

* How many users may be logged in concurrently?
* What are the expected/projected high high-traffic time periods?
* What will be the users’ regional distribution?
* What is the expected growth in number of users?
* What type of transactions will be conducted?
* How many projected transactions per day, per minute?

Desktop Application
-------------------
Integrating Widgets into a desktop application is not an ideal option. Rather the API should be consumed directly for those scenarios. We have had customers use the Widgets within a Windows application, in which the setup was as follows:

1. Windows application is installed on the desktop.
2. The application instantiates one or more instances of a web browser (Internet Explorer).
3. Within the app, IE pulls down a web page that integrates the Widgets.
4. The Widgets then hit the API to pull content.
5. Widgets render content.

This workflow is far from ideal. Most Widget visualizations are fairly standard (displaying headlines, for example). With a few exceptions, the visuals are not unique and could be recreated by a customer. In the case of a desktop application, rather than using the Widgets the API should be consumed directly and render the content within the application. For the most performance efficiency and best user experience the additional layers of the web browser and Widgets should not be used.

PERSONALIZATION
===============
If users will be saving their preferences, the implementation may require personalization support on the Dow Jones and Factiva platform. This will require the use of Session IDs (heavyweight sessions) when integrating, rather than encrypted tokens (lightweight sessions) for authentication. By getting this information up-front we can assess any integration difficulties and anticipate pain points that may be encountered. For example, expiring sessions or exceeding concurrent session rules.
 
As a rule, if the implementation does not require saving or tracking anything about individual users, lightweight sessions are the preferred choice. The following document will provide an overview of how to authenticate and help determine the best authentication for your solution: How to Authenticate

 
CUSTOMIZATION OF WIDGETS
========================
Heavy customization of Widgets, while supported, can lead to unintended performance issues. For instance, mashing the Radar Widget with market data can result in several API calls of varying duration that might result in a poor user experience. This question, coupled with assessing the skills of the development team, may assist anticipate any complications that might be encountered during development and integration.

 
FAQS
====

1. What are your expectations around page performance? For the web pages using either just the Dow Jones web services or the Widgets, what is the acceptable time for a page to load?

This question, coupled with the considerations regarding Customization of Widgets, can help us assess if the customer’s performance expectations are feasible with their reliance on JavaScript and Widgets.

2. If planning to use Dow Jones Widgets, what browsers are to be supported?

Older browsers will result in a poor user experience. Not just Widgets, but any website. We have ceased supporting IE6 by default, Widgets 2.0 supports IE7 and above.

3. Does your application support an authentication standard such as SAML or OAuth? How do you identify the individual users of your application?

We support several methods to identify a user (shortusername, longusername, emailaddress, others) that require some upfront work to configure for each customer. SAML support is coming soon.