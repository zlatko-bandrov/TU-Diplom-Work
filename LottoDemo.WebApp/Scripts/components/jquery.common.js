function showHint(e){var e=e;$j(e).siblings("div").css("display","block")}function hideHint(e){var e=e;$j(e).siblings("div").css("display","none")}function addToFavorites(e,n){if(window.sidebar)window.sidebar.addPanel(n,e,"");else if(window.external)window.external.AddFavorite(e,n);else if(window.opera&&window.print)return!0}function setDefaultHomepage(e){if(window.sidebar){try{netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect")}catch(n){return alert("Apologies but your browser (firefox) is preventing this action. Thank you."),!0}var r=Components.classes["@mozilla.org/preferences-service;1"].getService(Components.interfaces.nsIPrefBranch);return r.setCharPref("browser.startup.homepage",e),!0}if(window.external)document.body.style.behavior="url(#default#homepage)",document.body.setHomePage(e);else if(window.opera&&window.print)return!0}function sfOpenWindow(e,n,r){var t;return t=window.open(e,"popup","height="+n+",width="+r+",location=1,status=1,scrollbars=1,resizable=1"),window.focus?(t.focus(),!1):void 0}function arrayHasValue(e,n){for(var r=0;r<n.length;r++)if(n[r]==e)return!0;return!1}function getRadioGroupValue(e,n){for(var r=document.forms[n].elements[e],t=0;t<r.length;t++)if(1==r[t].checked)return r[t].value;return!1}function numberOnly(e){e=e?e:event;var n=e.charCode?e.charCode:e.keyCode?e.keyCode:e.which?e.which:0;return n>31&&(48>n||n>57)&&(37>n||n>40)?(alert("Enter numerals only in this field."),!1):!0}function decimalOnly(e){e=e?e:event;var n=e.charCode?e.charCode:e.keyCode?e.keyCode:e.which?e.which:0;return n>31&&(48>n||n>57)&&(37>n||n>40)&&46!=n?(alert("Decimal numerals only in this field."),!1):!0}function floatOnly(e,n){var r=$j("#"+n).val();e=e?e:event;var t=e.charCode?e.charCode:e.keyCode?e.keyCode:e.which?e.which:0;return t>31&&(48>t||t>57)&&(37>t||t>40)&&46!=t&&45!=t?(alert("Decimal numerals only in this field."),!1):!isDecimalRate(r,!0)||r.length>=1&&45==t?(alert("Decimal numerals only in this field."),!1):!0}function isNumber(e,n){var n=null==n||""==n?!1:n;if(1==n)var r=/[-]?\d*$/;else var r=/\d*$/;return e=e.toString(),e.match(r)?!0:!1}function isElmNumber(e){var n=$j("#"+e).val();return isNumber(n,!1)}function convertToInteger(e){return e=parseInt(e),isNaN(e)?!1:e}function convertElmToInteger(e,n,r,t){var n=null===n||""===n||isNaN(n)?0:convertToInteger(n),r=null===r||""===r||isNaN(r)?n:convertToInteger(r),t=null===t||""===t||isNaN(t)?!1:convertToInteger(t),i=convertToInteger($j("#"+e).val());return $j("#"+e).val(i===!1?n:r!==!1&&r>i?r:t!==!1&&i>t?t:i)}function isDecimalRate(e,n){if(1==n)var r=/^[-]?\d*\.?\d*$/;else var r=/^\d*\.?\d*$/;return e=e.toString(),e.match(r)?!0:!1}function isElmDecimalRate(e){var n=$j("#"+e).val();return result=isDecimalRate(n,!1)}function convertToFloat(e){return e=parseFloat(e),isNaN(e)?!1:e}function convertElmToRate(e,n,r,t){var n=null===n||""===n||isNaN(n)?0:convertToFloat(n),r=null===r||""===r||isNaN(r)?n:convertToFloat(r),t=null===t||""===t||isNaN(t)?!1:convertToFloat(t),i=convertToFloat($j("#"+e).val());return $j("#"+e).val(i===!1?n.toFixed(2):r!==!1&&r>i?r.toFixed(2):t!==!1&&i>t?t.toFixed(2):i.toFixed(2))}function isNotEmpty(e){var n=/.+/;return e.match(n)?!0:!1}function isElmNotEmpty(e){var n=$j("#"+e).val();return isNotEmpty(n)}function isLength(e,n){return e.length!=n?!1:!0}function isElmLength(e,n){var r=$j("#"+e).val();return isLength(r,n)}function showNowProcessing(){return tb_show(null,"index.php?TB_inline=true&height=150&width=340&inlineId=modalNowProcessing&modal=true",!0),!0}function checkNumberRange(e,n,r){return n>e||e>r?(alert("Number should be within the given range."),!1):!0}