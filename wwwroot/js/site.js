// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.popup-gallery').magnificPopup({
        removalDelay: 300,
        delegate: 'a',
        type: 'image',
        tLoading: 'Afbeelding #%curr% laden...',
        mainClass: 'mfp-img-mobile',
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
        },
        image: {
            tError: '<a href="%url%">The afbeelding #%curr%</a> kon niet geladen worden.',
            titleSrc: function (item) {
                return item.el.attr('title') + '<small>van de Krisnaldo\'s</small > ';
            }
        }
    });
});

$(document).ready(function () {
    $('#prijs-vorig-jaar').hide();
    $('#prijs-voorstel-vraag').hide();
    //$('#gsmnummer-bewerkt').hide();
    $("input[name='options']").change(function () {
        var test = $(this).val();

        if (test == 'nieuw') {
            $('#prijs-vorig-jaar').hide();
            $('#prijs-voorstel-vraag').hide();
        }
        else {
            $('#prijs-vorig-jaar').show();
            $('#prijs-voorstel-vraag').show();
        }
    });

    $('#gsmnummer-ingegeven').on('input', function () {
        var ar = []; 0
        var ar = $('#gsmnummer-ingegeven').val().split('');
        ar = ar.filter(function (str) { return /\S/.test(str); })
        var bewerkt = '';
        if (ar.length > 2) {
            bewerkt = bewerkt + ar[0] + ar[1] + ar[2];
        }
        if (ar.length > 4) {
            bewerkt = bewerkt + ' ' + ar[3] + ar[4];
        }
        if (ar.length > 6) {
            bewerkt = bewerkt + ' ' + ar[5] + ar[6];
        }
        if (ar.length > 8) {
            bewerkt = bewerkt + ' ' + ar[7] + ar[8];
        }
        $('#gsmnummer-bewerkt').text(bewerkt);
    });
});

$(document).ready(function () {
    $('#kalendertable_future_events').DataTable(
        {
            responsive: true,
            "ordering": false,
            "pagingType": "simple",
            "lengthMenu": [[10, -1], [10, "Alle"]],
            "language": {
                "sProcessing": "Bezig...",
                "sLengthMenu": "_MENU_ resultaten weergeven",
                "sZeroRecords": "Geen resultaten gevonden",
                "sInfo": "_START_ tot _END_ van _TOTAL_ resultaten",
                "sInfoEmpty": "Geen resultaten om weer te geven",
                "sInfoFiltered": " (gefilterd uit _MAX_ resultaten)",
                "sInfoPostFix": "",
                "sSearch": "Zoeken:",
                "sEmptyTable": "Geen resultaten aanwezig in de tabel",
                "sInfoThousands": ".",
                "sLoadingRecords": "Een moment geduld aub - bezig met laden...",
                "oPaginate": {
                    "sFirst": "Eerste",
                    "sLast": "Laatste",
                    "sNext": "Volgende",
                    "sPrevious": "Vorige"
                }
            }
        }
    );
});
$(document).ready(function () {
    $('#kalendertable_past_events').DataTable(
        {
            responsive: true,
            "ordering": false,
            "paging": false,
            "searching": false,
            "info":false,
            "language": {
                "sProcessing": "Bezig...",
                "sLengthMenu": "_MENU_ resultaten weergeven",
                "sZeroRecords": "Geen resultaten gevonden",
                "sInfo": "_START_ tot _END_ van _TOTAL_ resultaten",
                "sInfoEmpty": "Geen resultaten om weer te geven",
                "sInfoFiltered": " (gefilterd uit _MAX_ resultaten)",
                "sInfoPostFix": "",
                "sSearch": "Zoeken:",
                "sEmptyTable": "Geen resultaten aanwezig in de tabel",
                "sInfoThousands": ".",
                "sLoadingRecords": "Een moment geduld aub - bezig met laden...",
                "oPaginate": {
                    "sFirst": "Eerste",
                    "sLast": "Laatste",
                    "sNext": "Volgende",
                    "sPrevious": "Vorige"
                }
            }
        }
    );
});