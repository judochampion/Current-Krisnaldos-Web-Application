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
});