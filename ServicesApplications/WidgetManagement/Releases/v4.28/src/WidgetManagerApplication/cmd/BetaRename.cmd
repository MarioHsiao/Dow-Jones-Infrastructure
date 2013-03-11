echo off
rename E:\WIDGETS_WEB_SITE\config.xml			*.orig
rename E:\WIDGETS_WEB_SITE\reader\config.xml	*.orig
rename E:\WIDGETS_WEB_SITE\web.config			*.orig
rename E:\WIDGETS_WEB_SITE\Log4Net.config		*.orig
rename E:\WIDGETS_WEB_SITE\SiteConfiguration\data\SiteConfiguration.xml		*.orig
rename E:\WIDGETS_WEB_SITE\widgets_beta_not_on_this_servier.txt	beta_widgets..hold_aspx
echo Completed saving original config files

rem: change widgets.beta.factiva.com settings
copy E:\WIDGETS_WEB_SITE\config_beta.xml											E:\WIDGETS_WEB_SITE\config.xml
copy E:\WIDGETS_WEB_SITE\reader\config_beta.xml										E:\WIDGETS_WEB_SITE\reader\config.xml
copy E:\WIDGETS_WEB_SITE\web_beta.config											E:\WIDGETS_WEB_SITE\web.config
copy E:\WIDGETS_WEB_SITE\Log4Net_beta.config										E:\WIDGETS_WEB_SITE\Log4Net.config


copy E:\WIDGETS_WEB_SITE\iworks\bvd\config_beta.xml									E:\WIDGETS_WEB_SITE\iworks\bvd\config.xml
copy E:\WIDGETS_WEB_SITE\iworks\gym\config_beta.xml									E:\WIDGETS_WEB_SITE\iworks\gym\config.xml
copy E:\WIDGETS_WEB_SITE\iworks\reuters\config_beta.xml								E:\WIDGETS_WEB_SITE\iworks\reuters\config.xml
copy E:\WIDGETS_WEB_SITE\iworks\standard\config_beta.xml							E:\WIDGETS_WEB_SITE\iworks\standard\config.xml
copy E:\WIDGETS_WEB_SITE\iworks\wsj\config_beta.xml									E:\WIDGETS_WEB_SITE\iworks\wsj\config.xml

rename E:\WIDGETS_WEB_SITE\SiteConfiguration\data\SiteConfiguration_beta.xml		SiteConfiguration.xml
echo Completed Production set up

rem
echo on