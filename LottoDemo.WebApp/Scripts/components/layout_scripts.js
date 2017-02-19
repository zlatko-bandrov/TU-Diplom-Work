jQuery(document).ready(function($){


  $(".main-exp-nav").addClass("hideElement");
  $('.minicart-dropdown').click(function(){
		$('.cartShadow').toggleClass('cartShadowDropdown');
    $('.mini-cart-inner').toggleClass('mini-cart-show');
		return false;
	});

  $('.language-selector-block').click(function(){
    $('#l247-header-brand').toggleClass('brand-hidden');
    $('#l247-lang').toggleClass('lang-hidden');
    $('.main-header').toggleClass('lang-dropdown');
    $('.lang-selector-nav').toggleClass('lang-active');
		return false;
	});

    $(window).scroll(function() {
    var scroll = $(window).scrollTop();
     //console.log(scroll);
    if (scroll >= 1) {
        //console.log('a');
        $(".main-nav").addClass("menucont");
        $(".main-nav").removeClass("menuexp");
        $(".main-header__logo").addClass("main-header__logo_cont");
        $(".main-header").addClass("headerScrolled");
        $(".main-nav").addClass("navScrolled");
        $(".main-exp-nav").removeClass("hideElement");
        $(".main-exp-nav").removeClass("showElement");
        $('.site-menu-links').fadeOut();
    } else {
        //console.log('a');
        $(".main-nav").removeClass("menucont");
        $(".main-header__logo").removeClass("main-header__logo_cont");
        $(".main-exp-nav").addClass("hideElement");
        $(".main-header").removeClass("headerScrolled");
        $(".main-nav").addClass("menuexp");
        $(".main-nav").removeClass("navScrolled");
        $('.site-menu-links').fadeIn();
    }
});

  $('.newsHover-three').hover( function(event){
      $('.article-small-inner-two').toggleClass('article-small-inner-two-hover');
    });

  $('.newsHover-two').hover( function(event){
      $('.article-small-inner').toggleClass('article-small-inner-hover');
    });

    $('.newsHover').hover( function(event){
        $('.article-large-inner').toggleClass('article-large-inner-hover');
      });

    $(".dropdown").hover(
        function() { $('.dropdown-menu', this).stop().fadeIn("fast");
        },
        function() { $('.dropdown-menu', this).stop().fadeOut("fast");
        }
    );

    $('.dropdown-menu').click(function(e) {
        e.stopPropagation();
    });

    $('#submenu01').click( function(event){
        event.stopPropagation();
        $('#drop01').toggle();
        });

    $(document).click( function(){
        $('#drop01').hide();
        });

    $('#submenu02').click( function(event){
        event.stopPropagation();
        $('#drop02').toggle();
        });

    $(document).click( function(){
        $('#drop02').hide();
        });

    $('#submenu03').click( function(event){
        event.stopPropagation();
        $('#drop03').toggle();
        });

    $(document).click( function(){
        $('#drop03').hide();
        });

    $('#submenu04').click( function(event){
        event.stopPropagation();
        $('#drop04').toggle();
        });

    $(document).click( function(){
        $('#drop04').hide();
        });

    $('#submenu05').click( function(event){
        event.stopPropagation();
        $('#drop05').toggle();
        });

    $(document).click( function(){
        $('#drop05').hide();
        });



    /* Prevent Safari opening links when viewing as a Mobile App */
    (function(a, b, c) {
        if (c in b && b[c]) {
            var d, e = a.location,
                f = /^(a|html)$/i;
            a.addEventListener("click", function(a) {
                d = a.target;
                while (!f.test(d.nodeName))
                    d = d.parentNode;
                "href" in d && (d.href.indexOf("http") || ~d.href.indexOf(e.host)) && (a.preventDefault(), e.href = d.href)
            }, !1)
        }
    })(document, window.navigator, "standalone");
    /* Prevent Safari opening links when viewing as a Mobile App */



    /* Live Chat */
    try{checkOnlineOperators()}
    catch(err){return false;}
    /* Live Chat */

});
