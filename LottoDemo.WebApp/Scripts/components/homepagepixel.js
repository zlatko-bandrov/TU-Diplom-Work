/**
 * Created by gabriel on 2016/11/17.
 */

jQuery(document).ready(function() {
    homepagepixel();
});


function homepagepixel() {
    var affiliate_id = getCookie('affiliate_id');
    if(affiliate_id) {
        // fire off ajax request for homepage landing. a later cron will fire off a pixel for this when appropriate
        jQuery.ajax({
            type: 'POST',
            url: "/pixeltracking/index.html",
            data: {'affiliate_id': affiliate_id},
            //dataType: "json",
            success: function (data) {
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.log('error encountered');
                console.log(textStatus);
                console.log(errorThrown);
            }
        });
    }
}

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}
