

$(document).ready(function () {

 
    // Menu Types Options (see: jquery.menu.js for more details)
    var data = pageListItems;
    var publishedMenuOptions = {
        items: [
        {
            name: "<%=Token("unPublish")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/UnPublish/" + id;
                return false;
            }
        },
        {
            name: "<%=Token("edit")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                var title=$(gearIcon).parents('.tab-content').children('.label').html();
                $('.partialview-content', '#sample-admin-add-page-modal').load(($('#pathname').val()) + "AJAX/EditPage/"+id, function (response, status, xhr) {
                        if (status == "error") {
                                           alert("error");
                                              }
                        else {
                             $('#sample-admin-add-page-modal').removeClass("hide");
                             }

                    });

           }
        }, 
        {
            name: "<%=Token("delete")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/Delete/" + id;
                return false;
            }
        }
		]
    };



    var unPublishedMenuOptions = {
        items: [
        {
            name:  "<%=Token("publish")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/Publish/" + id;
                return false;
            }
        },
        {
            name: "<%=Token("edit")%>",
            onMouseDown: function (e) {
            
            var gearIcon = $(e.target).parents('.menu').data('targetElem');
            var id = $(gearIcon).parents('.tab-container').attr('id');
            var title=$(gearIcon).parents('.tab-content').children('.label').html();
                        $('.partialview-content', '#sample-admin-add-page-modal').load(($('#pathname').val()) + "AJAX/EditPage/"+id, function (response, status, xhr) {
                         if (status == "error") {
                                                alert("error");
                                                }
                        else {
                             $('#sample-admin-add-page-modal').removeClass("hide");
                             }

                      });
                                  }
        },
        {
            name: "<%=Token("delete")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/Delete/" + id;
                return false;
                                     }
        }
		]
    };

    var subscribedMenuOptions = {
        items: [{
            name: "<%=Token("copy")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/Copy/" + id;
                return false;
            }
        }, {
            name: "<%=Token("delete")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/Delete/" + id;
                return false;
            }
        }
		]
    };


    var assignedMenuOptions = {
        items: [{
            name: "<%=Token("copy")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/Copy/" + id;
                return false;
            }
        }, {
            name: "<%=Token("removeLabel")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/Remove/" + id;
                return false;
            }
        }
		]
    };






     var assignedSubscribeOnlyMenuOptions = {
        items: [{
            name: "<%=Token("removeLabel")%>",
            onMouseDown: function (e) {
                var gearIcon = $(e.target).parents('.menu').data('targetElem');
                var id = $(gearIcon).parents('.tab-container').attr('id');
                window.location = $('#pathname').val() + "Pages/Remove/" + id;
                return false;
            }
        }
		]
    };




  


    var regularMenuOptions = {
       
                             };

  var welcomeMenuOptions = {
         items: [ {
            name: "<%=Token("delete")%>",
            onMouseDown: function (e) {
              
                window.location = $('#pathname').val() + "Pages/EditWelcome";
                return false;
            }
        }
		]
                             };


    var flag = 0;
    var TabsArray = new Array();
    var activeTabPosition = 0;
    var selectItemTab = 1;
    function labels(name, cssclass, menuOptionsList, selectTabId, pageId) { this.label = name; this.tabClass = cssclass; this.menuOptions = menuOptionsList; this.selectTab = selectTabId; this.id = pageId }
     // alert($('#isSubscribeOn').val());
    //for active tab and selected tab to show as active
    $.each(data, function (index, optionData) {
        if (optionData.IsActive == true) { activeTabPosition = optionData.Position; selectItemTab = optionData.ID; }
       // alert("accsc- "+optionData.AccessScope+ "- pubsc -"+ optionData.PublishStatusScope);
    });
    //to show published or subscribed icon and menus
      // alert("in header "+$('#tabCount').val());
   // alert("act tab "+activeTabPosition);
    $.each(data, function (index, optionData) {
        //  alert(optionData.AccessScope);
        //AccessScope 0-ownedby user,1-AssignedToUser,2-SubscribedByUser

     //   alert("accsc- "+optionData.AccessScope+ "- pubsc -"+ optionData.PublishStatusScope);
        switch (optionData.AccessScope) {
            case 0:
                //alert(optionData.PublishStatusScope);
                //0-personal,1-account and 2-global
           
               if (optionData.PublishStatusScope == 0) 
               {
                               if(activeTabPosition==-1)
                               {
                               // for welcome
                                           TabsArray[TabsArray.length] = new labels(optionData.Title, "regularMenuOptions", welcomeMenuOptions, 0,0);
                                            activeTabPosition=0;
                                            selectItemTab=0;
                                          

                               }
                               else
                               {
                                       TabsArray[TabsArray.length] = new labels(optionData.Title, "regularMenuOptions", unPublishedMenuOptions, selectItemTab, optionData.ID);
           
                               }
                }
                else if (optionData.PublishStatusScope == 1)
                    {
                    TabsArray[TabsArray.length] = new labels(optionData.Title, "published", publishedMenuOptions, selectItemTab, optionData.ID);
                    }
                else if (optionData.PublishStatusScope == 2) 
                {
                    TabsArray[TabsArray.length] = new labels(optionData.Title, "regularMenuOptions", regularMenuOptions, selectItemTab, optionData.ID);
                    }
                break;
            case 1:
              
              if($('#isSubscribeOn').val())
              {
                   TabsArray[TabsArray.length] = new labels(optionData.Title, "regularMenuOptions", assignedSubscribeOnlyMenuOptions, selectItemTab, optionData.ID);
           
              }
              else
              {
                TabsArray[TabsArray.length] = new labels(optionData.Title, "regularMenuOptions", assignedMenuOptions, selectItemTab, optionData.ID);
             }
             
                break;
            case 2:
                TabsArray[TabsArray.length] = new labels(optionData.Title, "subscribed", subscribedMenuOptions, selectItemTab, optionData.ID);
                break;
            default:
                TabsArray[TabsArray.length] = new labels(optionData.Title, "regularMenuOptions", regularMenuOptions, selectItemTab, optionData.ID);
        }


    });

 
    if (activeTabPosition != 0)
        activeTabPosition = activeTabPosition - 1;
      //  alert("final act tab "+activeTabPosition);
    if ($('#isAd').val() == "True") {

        $("#tabbar").scrollableTabBar({
            activeTabIndex: activeTabPosition,
            tabs: TabsArray,
            actionItems: [ // Setting Up a Custom Action Item

			                {
			                id: 'admin-tab',
			                label: 'Admin',
			                onMouseDown: function (e) { window.location.href = $('#pathname').val() + "Admin/Index"; return false; }
			            }
                            ,
			                {
			                    id: 'header-toggle',
			                    iconClass: 'fi_expand',
			                    onMouseDown: function (e) { if ($("#header-content").hasClass('expanded')) { animateHeader('collapse'); } else { animateHeader('expand'); } }
			                }
		                ]
        });

    }
    else {
        $("#tabbar").scrollableTabBar({
            activeTabIndex: activeTabPosition,
            tabs: TabsArray,
            actionItems: [ // Setting Up a Custom Action Item

                      {
                      id: 'header-toggle',
                      iconClass: 'fi_expand',
                      onMouseDown: function (e) { if ($("#header-content").hasClass('expanded')) { animateHeader('collapse'); } else { animateHeader('expand'); } }
                  }
		            ]
        });

    }




    // Read the Header state from the cookie
    var headerState = $.cookie('headerState');
    // Apply the stored state
    if (headerState == 'expanded') {
        animateHeader('expand');
    }


    $("div .actionitems-container .actionitems-container-right").children("ul").children("li .actionitems").addClass("active");









});


//Updating the Edited Form

  $('#cancelUpdate').live('click', function (ev) {
    $('#sample-admin-add-page-modal').addClass("hide");
  });

  //For Updating edit Page
   $('#saveUpdate').live('click', function (ev) {
   var $form = $("#newsPageControl");
             $.ajax({
             type: "POST",
             url: $form.attr('action'),
             data: $form.serialize(),
             dataType: "json",
             success: function (data) {
               
               
                var tit=$('#RequestedNewsPage_Title').val();
                 $(".scrollabletabbar .tabs .tab-container.active .tab .label").text(tit);
                   
                 $('#sample-admin-add-page-modal').addClass("hide");
             }
         });
 return false;

  
  });

        $('#saveCreate').live('click', function (ev) {
    
        var tit=$('#RequestedNewsPage_Title').val();
        var desc=$('#RequestedNewsPage_Description').val();
        var title = new String(tit);
        if( title == null || title == undefined || title == "") { 
                    ev.preventDefault();
                    alert("Page Name should be entered and should be 25 characters max");
       
        }
        else
        {
            
            if(  title.length > 25 )
            {
                ev.preventDefault();
                alert("Page Name cannot be longer than 25 characters");
            
            
            }
        }
        if( desc != null || desc != undefined || desc != "")  
        { 
            var des = new String(desc);
            if(  des.length > 250 )
            {
               ev.preventDefault();
                alert("Page Description cannot be longer than 250 characters");
            
            }
        }
        if( hasIllegalChar(title) )
        {
            ev.preventDefault();
            alert("Page Name has illegal characters");
        }
        if( hasIllegalChar2(title) )
        {
            ev.preventDefault();
            alert("Page Name has illegal characters");
        }  
         if( hasIllegalChar3(title) )
        {
            ev.preventDefault();
            alert("Page Name has illegal characters");
        }      
  
  });
  //For wleocme page go action
  $('#welcomeSubmit').live('click', function (ev) {
         if($('#PageGardenList').val()=="")
         {
             alert("Please select Page");
         }
         else
         {
             window.location.href=$('#pathname').val() + "Pages/Copy/"+$('#PageGardenList').val();
         }
 });

 function validateTitleDescription(title, desc)
 {
    var valid = new Boolean();
    valid = false;

    if( title == null || title == undefined || title == "") { 
        valid = true;
        alert("Page Name should be entered and should be 25 characters max");
        
       // alert(valid);
    }
    else
    {
        var tit = new String(title);
        if(  tit.length > 25 )
        {
            valid = true;
            alert("Page Name cannot be longer than 25 characters");
            
            
        }
    }
    if( desc != null || desc != undefined || desc != "")  
    { 
        var des = new String(desc);
        if(  tit.length > 250 )
        {
            valid = true;
            alert("Page Description cannot be longer than 250 characters");
            
        }
    }
    //alert(valid);
    return valid;
    //alert("<%=Token("delete")%>");
 }

function hasIllegalChar(s) {
    return (s.match(/[<>&#\\%+|]/));
}

function hasIllegalChar2(s) {
    return (s.match(/[<>&#\\%+|~]/));
}

function hasIllegalChar3(s) {
    return (s.match(/[<>]/));
}
	
