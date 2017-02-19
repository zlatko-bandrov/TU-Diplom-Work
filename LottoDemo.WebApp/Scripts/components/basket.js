function openRemovalBox(element, jackpotType, lotteryId) {
    jQuery("#remove_" + element.id).toggle();
    jQuery("#numbers_default").toggle();
    jQuery("#lottery_entries_" + lotteryId).toggle("fast");

    return false;
}

function openManualRemovalBox(element, jackpotType, lotteryId) {
    jQuery("#manual_remove_" + element.id).toggle();
    jQuery("#manual_numbers_default").toggle();

    return false;
}

function changeRemoveEntry(lotteryId, jackpotType) {
    jQuery("#li_remove_" + jackpotType + "_" + lotteryId).toggle();
    return false;
}

function checkDuplicates() {
    jQuery.post('/api/validate-entries.html', {},
        function(data) {
            var response = JSON.parse(data);
            if (response.status == 'ERROR') {
                jQuery('#play_continue_button').hide();
                jQuery('#remove').prop('disabled', true);
                jQuery('.alert-danger').fadeIn('fast');
                alert('Please check for duplicate or empty entry lines in your basket before you proceed.');
                setTimeout(function(){
                    jQuery('.alert-danger').fadeOut('slow');
                }, 4000);
            } else {
                jQuery('#play_continue_button').show();
                jQuery('#remove').prop('disabled', false);;
            }


        });

}


function validateBasket(culture) {
    jQuery.post('/' + culture + '/checkout/basket/validate.html', {}, function (data) {
        var response = JSON.parse(data);
        if (response.status == 'ERROR') {
            jQuery('.activate_continue').hide();
            alert( jQuery('#basket_validation_message').html() );
        } else {
            //jQuery('.activate_continue').show();
        }
    });


}


function deleteLotteryEntry(lotteryId, lotteries, jackpotType, routeName, counter) {
    var countEntries  = 0, sfCulture = jQuery("html").attr('lang');

    jQuery.post('/checkout/removeentries.html', {
        lottery_id   : lotteryId,
        entry_count  : 1,
        jackpot_type : jackpotType,
        counter      : counter
    }, function (response) {
        if(!response.is_performance)
        {
            jQuery.post('/' + sfCulture + '/checkout/basket.html', {
                route_name: routeName
            }, function (responseHtml) {
                jQuery(".myShoppingBasket").html(responseHtml);
            });
        }



        jQuery.post('/' + sfCulture + '/checkout/basketAmount.html', {
            route_name: routeName
        }, function (responseHtml) {

            jQuery("#myShoppingBaskets").fadeIn('5600');
            jQuery.fn.shake = function(interval,distance,times){
                interval = typeof interval == "undefined" ? 100 : interval;
                distance = typeof distance == "undefined" ? 10 : distance;
                times = typeof times == "undefined" ? 3 : times;
                var jTarget = $(this);
                jTarget.css('position','relative');
                for(var iter=0;iter<(times+1);iter++){
                    jTarget.animate({ top: ((iter%2==0 ? distance : distance*-1))}, interval);

                }
                return jTarget.animate({ top: 0},interval);
            }
            jQuery(document).ready(function(){
                setTimeout(function(){
                    jQuery("#myShoppingBaskets").css({ 'color': '#50C2B4' }).shake(10, 10, 1);
                }, 30);
            });


            jQuery(".myShoppingBaskets").html(responseHtml);
            jQuery('.total-bold').html(responseHtml);
        });

        if (jQuery("#entry_str_" + lotteryId + "_" + jackpotType).length > 0) {
            jQuery("#entry_str_" + lotteryId + "_" + jackpotType).html(response.entry_str);
        }
        jQuery("#count_" + lotteryId).html(response.lottery_counter);

        jQuery("#" + lotteryId + "_" + jackpotType + "_" + counter).remove();
        if (response.lottery_counter === 0) {
            jQuery(".cart_counter").html(response.total_entries);
            jQuery("#entryHolder" + lotteryId).remove();
        }
        if (response.total_entries === 0) {
            document.location.reload();
        } else {
            jQuery.post('/' + sfCulture + '/checkout/lottery/entries.html', {
                lottery_id: lotteryId
            }, function (lotteryEntryHtml) {
                jQuery("#lottery_entries_" + lotteryId).html(lotteryEntryHtml);
            });

            checkDuplicates();
        }
    }, 'json');

}

function deleteEntry(element, lotteryId, lotteries, count, jackpotType, routeName, counter) {
    var countEntries  = 0, removeElement = jQuery("#remove_" + element.id), sfCulture = jQuery("html").attr('lang');
    checkDuplicates();
    if (count !== undefined) {
        countEntries = count;
    }
    if (removeElement.length > 0) {
        count = removeElement.val();
    }

    jQuery.post('/checkout/removeentries.html', {
        lottery_id   : lotteryId,
        entry_count  : count,
        jackpot_type : jackpotType,
        counter      : counter
    }, function (response) {
        if(!response.is_performance)
        {
            jQuery.post('/' + sfCulture + '/checkout/basket.html', {
                route_name: routeName
            }, function (responseHtml) {
                jQuery(".myShoppingBasket").html(responseHtml);
            });

        }

        jQuery.post('/' + sfCulture + '/checkout/basketAmount.html', {
            route_name: routeName
        }, function (responseHtml) {

            jQuery("#myShoppingBaskets").fadeIn('5600');
            jQuery(".total-bold").html(responseHtml);

        });

        if (countEntries === count) {
            jQuery(".cart_counter").html(lotteries - 1);
        }
        if(jQuery("#cart_summary").length > 0) {
            jQuery.post('/' + sfCulture + '/api/retrieve-basket.html', function (responseHtml) {
                jQuery("#cart_summary").html(responseHtml);
            });
        }
        if (jQuery("#basket").length > 0) {
            if ((lotteries === 1 && (count < 2 || countEntries === count))) {
                document.location.href = '/' + sfCulture + '/play-the-lottery.html';
            } else {
                jQuery.post('/' + sfCulture + '/checkout/index.html', function (responseHtml) {
                    jQuery("#basket").html(responseHtml);
                });
            }
        }
        if (jQuery("#entriesList").length > 0) {
            if ((lotteries === 1 && (count < 2 || countEntries === count))) {
                document.location.href = '/' + sfCulture + '/play-the-lottery.html';
            } else {
                jQuery.post('/' + sfCulture + '/play/entries.html', function (responseHtml) {
                    jQuery("#entriesList").html(responseHtml);
                });
            }
        }
        if (jQuery("#checkout-summary").length > 0) {
            reloadSummary(lotteries, count, countEntries);

        }

    });

}

function removeBundleBox(element) {
    jQuery("#span_" + element.id).toggle();
    return false;
}

function deleteBundleEntry(element, bundleType, bundleId, lotteries, count, routeName) {
    var remove_element = jQuery("#count_" + element.id), sfCulture     = jQuery("html").attr('lang');
    if (remove_element.length > 0) {
        count = remove_element.val();
    }
    jQuery.post('/checkout/removebundleentries.html', {
        bundle_id   : bundleId,
        bundle_type : bundleType,
        count       : count
    }, function (response) {
        jQuery(".cart_counter").html(response.total_entries);


        if (jQuery("#cart_summary").length > 0) {
            jQuery.post('/' + sfCulture + '/api/retrieve-basket.html', function (responseHtml) {
                jQuery("#cart_summary").html(responseHtml);
            });
        }
        jQuery.post('/' + sfCulture + '/checkout/basket.html', {
            route_name: routeName
        }, function (responseHtml) {
            jQuery(".myShoppingBasket").html(responseHtml);
        });

        if (jQuery("#basket").length > 0) {
            jQuery.post('/' + sfCulture + '/checkout/index.html', function (responseHtml) {
                jQuery("#basket").html(responseHtml);
            });
        }
        if (jQuery("#entriesList").length > 0) {
            jQuery.post('/' + sfCulture + '/play/entries.html', function (responseHtml) {
                jQuery("#entriesList").html(responseHtml);
            });
        }
        if (jQuery("#checkout-summary").length > 0) {
            reloadSummary(lotteries);
        }
    }, 'json');
}

function moveEntriesToNextDraw()
{
    var sfCulture = jQuery("html").attr('lang');
    jQuery.post("/"+sfCulture+"/checkout/move/entries/to/next/draw.html", function (response) {
        jQuery.post("/"+sfCulture+"/checkout/basket.html", function(response) {
            jQuery("#myShoppingBasket").replaceWith(response);
            document.location.href="/"+sfCulture+"/checkout/confirm.html";
        });
    });
}

function removeExpiredEntries()
{
    var sfCulture = jQuery("html").attr('lang');
    jQuery.post("/"+sfCulture+"/checkout/removeAllEntries.html", function (response) {
        jQuery.post("/"+sfCulture+"/checkout/basket.html", function(response) {
            jQuery("#myShoppingBasket").replaceWith(response);
            document.location.href="/"+sfCulture+"/play-the-lottery.html";
        });
    });
}