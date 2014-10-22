echo off
rename E:\WIDGETS_WEB_SITE\config.xml			*.orig
rename E:\WIDGETS_WEB_SITE\reader\config.xml	*.orig
rename E:\WIDGETS_WEB_SITE\web.config			*.orig
rename E:\WIDGETS_WEB_SITE\Log4Net.config		*.orig
rename E:\WIDGETS_WEB_SITE\SiteConfiguration\data\SiteConfiguration.xml		*.orig
rem copy   E:\WIDGETS_WEB_SITE\widgets_integration_not_on_this_servier.txt	E:\WIDGETS_WEB_SITE\int_widgets.hold
rename E:\WIDGETS_WEB_SITE\widgets_integration_not_on_this_servier.txt	int_widgets.hold_aspx
echo Completed saving original config files

rem change widgets.int.factiva.com settings
copy E:\WIDGETS_WEB_SITE\config_Integration.xml												E:\WIDGETS_WEB_SITE\config.xml
copy E:\WIDGETS_WEB_SITE\reader\config_Integration.xml										E:\WIDGETS_WEB_SITE\reader\config.xml
copy E:\WIDGETS_WEB_SITE\web_Integration.config												E:\WIDGETS_WEB_SITE\web.config
copy E:\WIDGETS_WEB_SITE\Log4Net_integration.config											E:\WIDGETS_WEB_SITE\Log4Net.config


copy E:\WIDGETS_WEB_SITE\iworks\bvd\config_Integration.xml									E:\WIDGETS_WEB_SITE\iworks\bvd\config.xml
copy E:\WIDGETS_WEB_SITE\iworks\gym\config_Integration.xml									E:\WIDGETS_WEB_SITE\iworks\gym\config.xml
copy E:\WIDGETS_WEB_SITE\iworks\reuters\config_Integration.xml								E:\WIDGETS_WEB_SITE\iworks\reuters\config.xml
copy E:\WIDGETS_WEB_SITE\iworks\standard\config_Integration.xml								E:\WIDGETS_WEB_SITE\iworks\standard\config.xml
copy E:\WIDGETS_WEB_SITE\iworks\wsj\config_Integration.xml									E:\WIDGETS_WEB_SITE\iworks\wsj\config.xml



rename E:\WIDGETS_WEB_SITE\SiteConfiguration\data\SiteConfiguration_integration.xml			SiteConfiguration.xml
echo Completed Integration set up

rem
echo on