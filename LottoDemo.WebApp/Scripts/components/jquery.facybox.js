;(function($j){$j.fn.fixPNG=function(){return this.each(function(){var image=$j(this).css('backgroundImage');if(image.match(/^url\(["']?(.*\.png)["']?\)$j/i)){image=RegExp.$j1;$j(this).css({'backgroundImage':'none','filter':"progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, sizingMethod="+($j(this).css('backgroundRepeat')=='no-repeat'?'crop':'scale')+", src='"+image+"')"}).each(function(){var position=$j(this).css('position');if(position!='absolute'&&position!='relative')
$j(this).css('position','relative');});}});};$j.facybox=function(data,klass){$j.facybox.loading();$j.facybox.content_klass=klass;if(data.ajax)revealAjax(data.ajax);else if(data.image)revealImage(data.image);else if(data.images)revealGallery(data.images,data.initial);else if(data.div)revealHref(data.div);else if($j.isFunction(data))data.call($j);else $j.facybox.reveal(data);}
$j.extend($j.facybox,{settings:{opacity:0.65,overlay:true,modal:false,imageTypes:['png','jpg','jpeg','gif']},html:function(){return'\
  <div id="facybox" style="display:none;"> \
   <div class="popup"> \
    <table> \
     <tbody> \
      <tr> \
       <td class="nw"/><td class="n" /><td class="ne"/> \
      </tr> \
      <tr> \
       <td class="w" /> \
       <td class="body"> \
       <div class="footer"> </div> \
       <a href="#" class="close"></a>\
       <div class="content"> \
       </div> \
      </td> \
       <td class="e"/> \
      </tr> \
      <tr> \
       <td class="sw"/><td class="s"/><td class="se"/> \
      </tr> \
     </tbody> \
    </table> \
   </div> \
  </div> \
  <div class="loading"></div> \
 '},loading:function(){init();if($j('.loading',$j('#facybox'))[0])return;showOverlay();$j.facybox.wait();if(!$j.facybox.settings.modal){$j(document).bind('keydown.facybox',function(e){if(e.keyCode==27)$j.facybox.close();});}
$j(document).trigger('loading.facybox');},wait:function(){var $jf=$j('#facybox');$j('.content',$jf).empty();$j('.body',$jf).children().hide().end().append('<div class="loading"></div>');$jf.fadeIn('fast');$j.facybox.centralize();$j(document).trigger('reveal.facybox').trigger('afterReveal.facybox');},centralize:function(){var $jf=$j('#facybox');var pos=$j.facybox.getViewport();var wl=parseInt(pos[0]/2)-parseInt($jf.find("table").width()/2);var fh=parseInt($jf.height());if(pos[1]>fh){var t=(pos[3]+(pos[1]-fh)/2);$jf.css({'left':wl,'top':t});}else{var t=(pos[3]+(pos[1]/10));$jf.css({'left':wl,'top':t});}},getViewport:function(){return[$j(window).width(),$j(window).height(),$j(window).scrollLeft(),$j(window).scrollTop()];},reveal:function(content){$j(document).trigger('beforeReveal.facybox');var $jf=$j('#facybox');$j('.content',$jf).attr('class',($j.facybox.content_klass||'')+' content').html(content);$j('.loading',$jf).remove();var $jbody=$j('.body',$jf);$jbody.children().fadeIn('fast');$j.facybox.centralize();$j(document).trigger('reveal.facybox').trigger('afterReveal.facybox');},close:function(){$j(document).trigger('close.facybox');return false;}})
$j.fn.facybox=function(settings){var $jthis=$j(this);if(!$jthis[0])return $jthis;if(settings)$j.extend($j.facybox.settings,settings);if(!$j.facybox.settings.noAutoload)init();$jthis.bind('click.facybox',function(){$j.facybox.loading();var klass=this.rel.match(/facybox\[?\.(\w+)\]?/);$j.facybox.content_klass=klass?klass[1]:'';revealHref(this.href);return false;});return $jthis;}
function init(){if($j.facybox.settings.inited)return;else $j.facybox.settings.inited=true;$j(document).trigger('init.facybox');makeBackwardsCompatible();var imageTypes=$j.facybox.settings.imageTypes.join('|');$j.facybox.settings.imageTypesRegexp=new RegExp('\.('+imageTypes+')','i');$j('body').append($j.facybox.html());var $jf=$j("#facybox");if($j.browser.msie){$j(".n, .s, .w, .e, .nw, .ne, .sw, .se",$jf).fixPNG();if(parseInt($j.browser.version)<=6){var css="<style type='text/css' media='screen'>* html #facybox_overlay { position: absolute; height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');}</style>"
$j('head').append(css);$j(".close",$jf).fixPNG();$j(".close",$jf).css({'right':'15px'});}
$j(".w, .e",$jf).css({width:'13px','font-size':'0'}).text("�");}
if(!$j.facybox.settings.noAutoload){preloadImages();}
$j('#facybox .close').click($j.facybox.close);}
function preloadImages(){$j('#facybox').find('.n, .close , .s, .w, .e, .nw, ne, sw, se').each(function(){var img=new Image();img.src=$j(this).css('background-image').replace(/url\((.+)\)/,'$j1');})
var img=new Image();img.src='/images/fancybox/loading.gif';}
function makeBackwardsCompatible(){var $js=$j.facybox.settings;$js.imageTypes=$js.image_types||$js.imageTypes;$js.facyboxHtml=$js.facybox_html||$js.facyboxHtml;}
function revealHref(href){if(href.match(/#/)){var url=window.location.href.split('#')[0];var target=href.replace(url,'');if(target=='#')return
$j.facybox.reveal($j(target).html(),$j.facybox.content_klass);}else if(href.match($j.facybox.settings.imageTypesRegexp)){revealImage(href);}else{revealAjax(href)}}
function revealGallery(hrefs,initial){var position=$j.inArray(initial||0,hrefs);if(position==-1){position=0;}
var $jfooter=$j('#facybox div.footer');$jfooter.append($j('<div class="navigation"><a class="prev"/><a class="next"/><div class="counter"></div></div>'));var $jnav=$j('#facybox .navigation');$j(document).bind('afterClose.facybox',function(){$jnav.remove()});function change_image(diff){position=(position+diff+hrefs.length)%hrefs.length;revealImage(hrefs[position]);$jnav.find('.counter').html(position+1+" / "+hrefs.length);}
change_image(0);$j('.prev',$jnav).click(function(){change_image(-1)});$j('.next',$jnav).click(function(){change_image(1)});$j(document).bind('keydown.facybox',function(e){if(e.keyCode==39)change_image(1);if(e.keyCode==37)change_image(-1);});}
function revealImage(href){var $jf=$j("#facybox");$j('#facybox .content').empty();$j.facybox.loading();var image=new Image();image.onload=function(){$j.facybox.reveal('<div class="image"><img src="'+image.src+'" /></div>',$j.facybox.content_klass);var $jfooter=$j("div.footer",$jf);var $jcontent=$j("div.content",$jf);var $jnavigation=$j("div.navigation",$jf);var $jnext=$j("a.next",$jf);var $jprev=$j("a.prev",$jf);var $jcounter=$j("div.counter",$jf);var size=[$jcontent.width(),$jcontent.height()];$jfooter.width(size[0]).height(size[1]);$jnavigation.width(size[0]).height(size[1]);$jnext.width(parseInt(size[0]/2)).height(size[1]).css({left:(size[0]/2)});$jprev.width(size[0]/2).height(size[1]);$jcounter.width(parseInt($jf.width()-26)).css({'opacity':0.5,'-moz-border-radius':'8px','-webkit-border-radius':'8px'})}
image.src=href;}
function revealAjax(href){$j.get(href,function(data){$j.facybox.reveal(data)});}
function skipOverlay(){return $j.facybox.settings.overlay==false||$j.facybox.settings.opacity===null}
function showOverlay(){if(skipOverlay())return;if($j('#facybox_overlay').length==0){$j("body").append('<div id="facybox_overlay" class="facybox_hide"></div>');}
$j('#facybox_overlay').hide().addClass("facybox_overlayBG").css('opacity',$j.facybox.settings.opacity).fadeIn(200);if(!$j.facybox.settings.modal){$j('#facybox_overlay').click(function(){$j(document).trigger('close.facybox')})}}
function hideOverlay(){if(skipOverlay())return;$j('#facybox_overlay').fadeOut(200,function(){$j("#facybox_overlay").removeClass("facybox_overlayBG").addClass("facybox_hide").remove();})}
$j(document).bind('close.facybox',function(){$j(document).unbind('keydown.facybox');var $jf=$j("#facybox");if($j.browser.msie){$j('#facybox').hide();hideOverlay();$j('#facybox .loading').remove();}else{$j('#facybox').fadeOut('fast',function(){$j('#facybox .content').removeClass().addClass('content');hideOverlay();$j('#facybox .loading').remove();})}
$j(document).trigger('afterClose.facybox');$j("object").css("display", "block");$j("embed").css("display", "block");});})(jQuery);