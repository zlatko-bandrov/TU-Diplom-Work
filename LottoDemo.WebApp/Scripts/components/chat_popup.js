
var default_chat_dpt = 'CC';

function timestamp() {
    var date = new Date();
    return date.getTime();
}

function launchChatWindow(is_live_chat_window, department)
{
    if(typeof(department) == "undefined") department = default_chat_dpt;
    var request_url = "http://support.playukinternet.com/support/users/tickets.php?op=add&dpt="+department;
    var mywidth   = 780;
    var myheight  = 650;
    var myResizable = 0;

    if(is_live_chat_window)
    {
        request_url = "/sfChat/register.html?dpt="+department;
        mywidth   = 780;
        myheight  = 635;
        myResizable = 1;
    }

    var settings = "status=0,toolbar=0,location=0,resizable=" + myResizable + " , menubar=0,scrollbars=1,width=" + mywidth + ",height=" + myheight;
    //var new_win = window.open(request_url, timestamp(), settings);
    var new_win = window.open(request_url, '_blank', settings);
    //new_win.focus();
}

function launchChatWindow_lottarewards(is_live_chat_window, department)
{
    var department = "AFF";
    //if(typeof(department) == "undefined") department = "AFF";
    var request_url = "http://support.playukinternet.com/support/users/tickets.php?op=add&dpt="+department;
    var mywidth   = 780;
    var myheight  = 600;

    if(is_live_chat_window)
    {
        request_url = "/sfChat/register/dpt="+department;
        mywidth   = 780;
        myheight  = 635;
    }

    var settings = "status=0,toolbar=0,location=0,menubar=0,scrollbars=1,width=" + mywidth + ",height=" + myheight;
    //var new_win = window.open(request_url, timestamp(), settings);
    var new_win = window.open(request_url, '_blank', settings);
    //new_win.focus();
}


var previous_status = "";

function checkOnlineOperators()
{
    var department = default_chat_dpt;
    if(jQuery("#aLiveHelp[rel='SLS']").length > 0) department = 'CC';

    var data = { "mode" : "checkOnlineOperators", "dpt": department }
    var params =  jQuery.toJSON(data);

    jQuery.post("/sfChat/update.html", { data: params }, displayOperatorStatus2, "text");
    //setTimeout("checkOnlineOperators();", 30000);
}

function checkOnlineOperators_lotto_rewards()
{
    var department = "AFF";
    if(jQuery("#aLiveHelp[rel='SLS']").length > 0) department = 'AFF';

    var data = { "mode" : "checkOnlineOperators", "dpt": department }
    var params =  jQuery.toJSON(data);


    jQuery.post("/sfChat/update", { data: params }, displayOperatorStatus_lottarewards, "text");
    //setTimeout("checkOnlineOperators();", 30000);
}

function displayOperatorStatus(res)
{
    var obj = jQuery.evalJSON(res);
    if(obj.status != previous_status)
    {
        previous_status = obj.status;
        if(jQuery(".imgLiveSupport").length > 0){
            jQuery(".imgLiveSupport").attr("src", "/sfLiveChatPlugin/images/icons/" + obj.available + "-" + obj.culture + ".svg");

            if(obj.available == "online"){
                jQuery("#iLiveSupport").removeClass('offline');
            }
            if(obj.status == "online"){
                jQuery("#iLiveSupport").click(function(e){
                    jQuery('#iLiveSupport > a').on('click', liveCCChatHelp());
                });

                jQuery("#headLiveSupport").removeClass('offline');
            }

            jQuery(".imgLiveSupport").click(function(e){



                if(obj.status == "online"){
                    var dpt = default_chat_dpt;
                    if(typeof(jQuery(this).attr("rel")) != "undefined"){
                        dpt = jQuery(this).attr("rel");
                    }
                    launchChatWindow(true, dpt);
                    e.preventDefault();
                    return false;
                }
                else
                {
                    window.location = "/"+obj.culture+"/support/edit.html";
                    return false;
                }
            });

            jQuery("#headLiveSupport").click(function(e){
                if(obj.status == "online"){
                    var dpt = default_chat_dpt;
                    if(typeof(jQuery(this).attr("rel")) != "undefined"){
                        dpt = jQuery(this).attr("rel");
                    }
                    launchChatWindow(true, dpt);
                    e.preventDefault();
                    return false;
                }
                else
                {
                    window.location = "/"+obj.culture+"/support/edit.html";
                    return false;
                }
            });
        }
    }
}


function displayOperatorStatus2(res)
{
    var obj = jQuery.evalJSON(res);
    if(obj.status != previous_status)
    {
        // obj.available = 'online'
        previous_status = obj.status;
        if(jQuery(".imgLiveSupport").length > 0){
            jQuery(".imgLiveSupport").attr("src", "/images/frontend/live-support-" + obj.available + "-chat" + ".svg");

            if(obj.available=='online'){
                colour = 'green';
				text = 'Online'
                // jQuery("#status-colour").text('online');
            }else{
                colour = 'red';
				text = 'Offline'
                // jQuery("#status-colour").text('offline');
            }

            jQuery('#status-colour').css({"color":colour}).text(text);
            
            //Lotto247
            if (colour == 'red') {
                jQuery('#l247_livechat_online').hide();
                jQuery('#l247_livechat_offline').fadeIn();
            } else {
                jQuery('#l247_livechat_online').fadeIn();
                jQuery('#l247_livechat_offline').hide();
            }

            if(obj.available == "online"){
                jQuery("#iLiveSupport").removeClass('offline');
            }
            if(obj.status == "online"){
                jQuery("#iLiveSupport").click(function(e){
                    jQuery('#iLiveSupport > a').on('click', liveCCChatHelp());
                });

                jQuery("#headLiveSupport").removeClass('offline');
            }

            jQuery(".imgLiveSupport").click(function(e){



                if(obj.status == "online"){
                    var dpt = default_chat_dpt;
                    if(typeof(jQuery(this).attr("rel")) != "undefined"){
                        dpt = jQuery(this).attr("rel");
                    }
                    launchChatWindow(true, dpt);
                    e.preventDefault();
                    return false;
                }
                else
                {
                    window.location = "/"+obj.culture+"/support/edit.html";
                    return false;
                }
            });

            jQuery("#headLiveSupport").click(function(e){
                if(obj.status == "online"){
                    var dpt = default_chat_dpt;
                    if(typeof(jQuery(this).attr("rel")) != "undefined"){
                        dpt = jQuery(this).attr("rel");
                    }
                    launchChatWindow(true, dpt);
                    e.preventDefault();
                    return false;
                }
                else
                {
                    window.location = "/"+obj.culture+"/support/edit.html";
                    return false;
                }
            });
        }
    }
}

function displayOperatorStatus_lottarewards(res)
{
    var obj = jQuery.evalJSON(res);
    if(obj.status != previous_status)
    {
        previous_status = obj.status;
        if(jQuery(".imgLiveSupport").length > 0){
            jQuery(".imgLiveSupport").attr("src", "/sfLiveChatPlugin/images/icons/" + obj.available + "-" + obj.culture + ".svg");

            if(obj.available == "online"){
                jQuery("#iLiveSupport").removeClass('offline');
            }
            if(obj.status == "online"){
                jQuery("#iLiveSupport").click(function(e){
                    jQuery('#iLiveSupport > a').on('click', liveCCChatHelp_lottarewards());
                });

                jQuery("#headLiveSupport").removeClass('offline');
            }

            jQuery(".imgLiveSupport").click(function(e){
                if(obj.status == "online"){
                    var dpt = "AFF";
                    if(typeof(jQuery(this).attr("rel")) != "undefined"){
                        dpt = jQuery(this).attr("rel");
                    }
                    launchChatWindow_lottarewards(true, dpt);
                    e.preventDefault();
                    return false;
                }
                else
                {
                    window.location = "/"+obj.culture+"/support/edit.html";
                    return false;
                }
            });

            jQuery("#headLiveSupport").click(function(e){
                if(obj.status == "online"){
                    var dpt = "AFF";
                    if(typeof(jQuery(this).attr("rel")) != "undefined"){
                        dpt = jQuery(this).attr("rel");
                    }
                    launchChatWindow_lottarewards(true, dpt);
                    e.preventDefault();
                    return false;
                }
                else
                {
                    window.location = "/"+obj.culture+"/support/edit.html";
                    return false;
                }
            });
        }
    }
}

function isOperatorOnline(res)
{
    var obj = jQuery.evalJSON(res);

    previous_status = obj.status;
    if(obj.status == "online"){
        var dpt = 'CC';
        if(typeof(jQuery(this).attr("rel")) != "undefined"){
            dpt = jQuery(this).attr("rel");
        }
        launchChatWindow(true, dpt);
        e.preventDefault();
        return false;
    }
    else
    {
        alert('No operators are currently available for Live Support. Please try again later.');
        return false;
    }
}

function launchAdminLiveChatWindow(department)
{
    if(typeof(department) == "undefined") department = default_chat_dpt;
    var settings = "status=0,toolbar=0,resizable=0,location=0,menubar=0,scrollbars=1,width=955,height=675,left=150,top=150";
    var w=window.open('/backend.php/sfChat/admin?dpt='+department, 'myWindow'+department, settings);
    w.focus();
}

function liveChatHelp()
{
    launchChatWindow(true, 'CC');
    return false;
}

function liveCCChatHelp()
{
    launchChatWindow(true, 'CC');
    return false;
}

function liveCCChatHelp_lottarewards()
{
    launchChatWindow_lottarewards(true, 'AFF');
    return false;
}

function liveCCChatHelpLink()
{
    var department = 'CC';

    var data = { "mode" : "checkOnlineOperators", "dpt": department }
    var params =  jQuery.toJSON(data);

    jQuery.post("/sfChat/update.html", { data: params }, isOperatorOnline, "text");

    return false;
}
