"use strict!"
$(document).ready(function () {
    var noofdays = 1;
    var Navbarbg = "theme1";
    var headerbg = "themelight1";
    var menucaption = "theme1";
    var bgpattern = "theme1";
    var activeitemtheme = "theme1";
    var frametype = "theme1";
    var layout_type = "light";
    var layout_width = "wide";
    var menu_effect_desktop = "shrink";
    var menu_effect_tablet = "overlay";
    var menu_effect_phone = "overlay";
    var menu_icon_style = "st2";

    $("#pcoded").pcodedmenu(
        {
            themelayout: 'vertical',
            verticalMenuplacement: 'left',
            verticalMenulayout: layout_width,
            MenuTrigger: 'click',
            SubMenuTrigger: 'click',
            activeMenuClass: 'active',
            ThemeBackgroundPattern: bgpattern,
            HeaderBackground: headerbg,
            LHeaderBackground: menucaption,
            NavbarBackground:
                Navbarbg,
            ActiveItemBackground:
                activeitemtheme,
            SubItemBackground: 'theme2',
            LogoTheme: 'theme6',
            ActiveItemStyle: 'style0',
            ItemBorder: true,
            ItemBorderStyle: 'solid',
            freamtype: frametype,
            SubItemBorder: false,
            DropDownIconStyle: 'style1',
            menutype: menu_icon_style,
            layouttype: layout_type,
            FixedNavbarPosition: false,
            FixedHeaderPosition: false,
            collapseVerticalLeftHeader: true,
            VerticalSubMenuItemIconStyle: 'style1',
            VerticalNavigationView: 'view1',
            verticalMenueffect: {
                desktop: menu_effect_desktop, tablet: menu_effect_tablet, phone: menu_effect_phone,
            }, defaultVerticalMenu: {
                desktop: "expanded", tablet: "offcanvas", phone: "offcanvas",
            }, onToggleVerticalMenu: { desktop: "offcanvas", tablet: "expanded", phone: "expanded", },
        });
    function handlelayouttheme() {
        $('.theme-color > a.Layout-type').on("click",
            function () {
                var layout = $(this).attr("layout-type");
                $('.pcoded').attr("layout-type", layout);
                if (layout == 'dark') {
                    $('.pcoded-header').attr("header-theme", "themelight1");
                    $('.pcoded-navbar').attr("navbar-theme", "theme1");
                    $('.pcoded-navbar').attr("active-item-theme", "theme1");
                    $('.pcoded').attr("fream-type", "theme1");
                    $('body').addClass('dark');
                    $('body').attr("themebg-pattern", "theme1");
                    $('.pcoded-navigation-label').attr("menu-title-theme", "theme1");
                }
                if (layout == 'light') {
                    $('.pcoded-header').attr("header-theme", "themelight1");
                    $('.pcoded-navbar').attr("navbar-theme", "themelight1");
                    $('.pcoded-navigation-label').attr("menu-title-theme", "theme2");
                    $('.pcoded-navbar').attr("active-item-theme", "theme1");
                    $('.pcoded').attr("fream-type", "theme1");
                    $('body').removeClass('dark');
                    $('body').attr("themebg-pattern", "theme1");
                }
            });
    }; handlelayouttheme();
    function handleleftheadertheme() {
        $('.theme-color > a.leftheader-theme').on("click",
            function () {
                var lheadertheme = $(this).attr("menu-caption");
                $('.pcoded-navigation-label').attr("menu-title-theme", lheadertheme);
            });
    }; handleleftheadertheme();
    function handleheaderthemefull() {
        $('.theme-color > a.header-theme-full').on("click",
            function () {
                var headertheme = $(this).attr("header-theme");
                var activeitem = $(this).attr("active-item-color");
                $('.pcoded-header').attr("header-theme", headertheme);
                $('.navbar-logo').attr("logo-theme", headertheme);
                $('.pcoded-navbar').attr("active-item-theme", activeitem);
                $('.pcoded').attr("fream-type", headertheme);
                $('body').attr("themebg-pattern", headertheme);
                if (headertheme == "themelight1") {
                    $('.pcoded-navbar').attr("active-item-theme", "theme1");
                }
            });
    }; handleheaderthemefull();
    function handleheadertheme() {
        $('.theme-color > a.header-theme').on("click",
            function () {
                var headertheme = $(this).attr("header-theme");
                var activeitem = $(this).attr("active-item-color");
                $('.pcoded-header').attr("header-theme", headertheme);
                $('.pcoded-navbar').attr("active-item-theme", activeitem);
                $('.pcoded').attr("fream-type", headertheme);
                $('body').attr("themebg-pattern", headertheme);
                if (headertheme == "themelight1") {
                    $('.pcoded-navbar').attr("active-item-theme", "theme1");
                }
            });
    }; handleheadertheme();
    function handlenavbartheme() {
        $('.theme-color > a.navbar-theme').on("click",
            function () {
                var navbartheme = $(this).attr("navbar-theme");
                $('.pcoded-navbar').attr("navbar-theme", navbartheme);
                if (navbartheme == 'themelight1') {
                    $('.pcoded-navigation-label').attr("menu-title-theme", "theme2");
                }
                if (navbartheme == 'theme1') {
                    $('.pcoded-navigation-label').attr("menu-title-theme", "theme1");
                }
            });
    }; handlenavbartheme();
    function handleactiveitemtheme() {
        $('.theme-color > a.active-item-theme').on("click",
            function () {
                var activeitemtheme = $(this).attr("active-item-theme");
                $('.pcoded-navbar').attr("active-item-theme", activeitemtheme);
            });
    }; handleactiveitemtheme();
    function handlethemebgpattern() {
        $('.theme-color > a.themebg-pattern').on("click",
            function () {
                var themebgpattern = $(this).attr("themebg-pattern");
                $('body').attr("themebg-pattern", themebgpattern);
            });
    }; handlethemebgpattern();
    function handlethemeverticallayout() {
        $('#theme-layout').change(
            function () {
                if ($(this).is(":checked")) {
                    $('.pcoded').attr('vertical-layout', "box");
                    $('#bg-pattern-visiblity').removeClass('d-none');
                }
                else {
                    $('.pcoded').attr('vertical-layout', "wide");
                    $('#bg-pattern-visiblity').addClass('d-none');
                }
            });
    }; handlethemeverticallayout();
    function handleverticalMenueffect() {
        $('#vertical-menu-effect').val('shrink').on('change',
            function (get_value) {
                get_value = $(this).val();
                $('.pcoded').attr('vertical-effect', get_value);
            });
    }; handleverticalMenueffect();
    function handleverticalboderstyle() {
        $('#vertical-border-style').val('solid').on('change',
            function (get_value) {
                get_value = $(this).val();
                $('.pcoded-navbar .pcoded-item').attr('item-border-style', get_value);
            });
    }; handleverticalboderstyle();
    function handleVerticalDropDownIconStyle() {
        $('#vertical-dropdown-icon').val('style1').on('change',
            function (get_value) {
                get_value = $(this).val();
                $('.pcoded-navbar .pcoded-hasmenu').attr('dropdown-icon', get_value);
            });
    }; handleVerticalDropDownIconStyle();
    function handleVerticalSubMenuItemIconStyle() {
        $('#vertical-subitem-icon').val('style5').on('change',
            function (get_value) {
                get_value = $(this).val();
                $('.pcoded-navbar .pcoded-hasmenu').attr('subitem-icon', get_value);
            });
    }; handleVerticalSubMenuItemIconStyle();
    function handlesidebarposition() {
        $('#sidebar-position').change(
            function () {
                if ($(this).is(":checked")) {
                    $('.pcoded-navbar').attr("pcoded-navbar-position", 'fixed');
                    $('.pcoded-header .pcoded-left-header').attr("pcoded-lheader-position", 'fixed');
                }
                else {
                    $('.pcoded-navbar').attr("pcoded-navbar-position", 'absolute');
                    $('.pcoded-header .pcoded-left-header').attr("pcoded-lheader-position", 'relative');
                }
            });
    }; handlesidebarposition();
    function handleheaderposition() {
        $('#header-position').change(
            function () {
                if ($(this).is(":checked")) {
                    $('.pcoded-header').attr("pcoded-header-position", 'fixed');
                    $('.pcoded-navbar').attr("pcoded-header-position", 'fixed');
                    $('.pcoded-main-container').css('margin-top', $(".pcoded-header").outerHeight());
                }
                else {
                    $('.pcoded-header').attr("pcoded-header-position", 'relative');
                    $('.pcoded-navbar').attr("pcoded-header-position", 'relative');
                    $('.pcoded-main-container').css('margin-top', '0px');
                }
            });
    }; handleheaderposition();
    function handlecollapseLeftHeader() {
        $('#collapse-left-header').change(
            function () {
                if ($(this).is(":checked")) {
                    $('.pcoded-header, .pcoded ').removeClass('iscollapsed');
                    $('.pcoded-header, .pcoded').addClass('nocollapsed');
                }
                else {
                    $('.pcoded-header, .pcoded').addClass('iscollapsed');
                    $('.pcoded-header, .pcoded').removeClass('nocollapsed');
                }
            });
    }; handlecollapseLeftHeader();
});
function handlemenutype(get_value) {
    $('.pcoded').attr('nav-type', get_value);
}; handlemenutype("st2");