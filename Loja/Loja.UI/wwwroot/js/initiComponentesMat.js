(function () {
    //dropdown
    document.addEventListener('DOMContentLoaded', function () {
        var elems = document.querySelectorAll('.dropdown-trigger');
        var instances = M.Dropdown.init(elems);
    });
    //select
    document.addEventListener('DOMContentLoaded', function () {
        var elems = document.querySelectorAll('select');
        var instances = M.FormSelect.init(elems);
    });
    //modal
    document.addEventListener('DOMContentLoaded', function () {
        var elems = document.querySelectorAll('.modal');
        var instances = M.Modal.init(elems);
    });
    //sidenav
    document.addEventListener('DOMContentLoaded', function () {
        var elems = document.querySelectorAll('.sidenav');
        var instances = M.Sidenav.init(elems);
    });
    //document.addEventListener('DOMContentLoaded', function () {
    //    var elems = document.querySelectorAll('.collapsible');
    //    var instances = M.Collapsible.init(elems,options);
    //});
    document.addEventListener('DOMContentLoaded', function () {
        var elems = document.querySelector('.autocomplete');
        var instances = M.Autocomplete.init(elems);
    });
    //datepicker
    document.addEventListener('DOMContentLoaded', function () {
        var elems = document.querySelectorAll('.datepicker');
        var instances = M.Datepicker.init(elems);
    });
})();

//esta sendo feito em jquery pois com javascript não estava funcionando corretamente
$(document).ready(function () {
    $('.collapsible').collapsible();
});


