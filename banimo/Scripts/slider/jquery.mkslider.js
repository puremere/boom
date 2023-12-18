/*
MKSlider v1.0.0
Copyright (c) 2015 Mikhail Kelner
Released under the MIT License <http://www.opensource.org/licenses/mit-license.php>
*/
(function($){
	jQuery.fn.mkslider = function(options){
		var options = $.extend({
			auto: true,
			autoTimeout: 2000,
			autoIcon: true,
			autoIconPlay: '►',
			autoIconPause: '&#9646;&#9646;',
			animationTime: 1000,
			animationPerSlide: true,
			arrows: true,
			arrowNext: '→',
			arrowPrev: '←',
			navigator: true,
			navigatorItemText: '●',
			externalNavigator: '',
		}, options);
		var $object;
		var $innerWrapper;
		var $navigator;
		var $externalNavigator;
		var isAutoEnabled = false;
		var make = function(){
			$object = $(this);
			$object.addClass('mkslider');
			$object.wrapInner('<div class="mkslider-content-inner-wrapper" />');
			$object.wrapInner('<div class="mkslider-content-outer-wrapper" />');
			$innerWrapper = $object.find('.mkslider-content-inner-wrapper')
			var $outerWrapper = $object.find('.mkslider-content-outer-wrapper')
			var $slides = $innerWrapper.find('> *');
			$slides.addClass('mkslider-slide').css('width','100%');
			$object.attr('data-mkslider-slide',0);
			$object.attr('data-mkslider-slides',$slides.length);
			$innerWrapper.css({
				'width': (100 * $slides.length) + '%',
				'position': 'relative'
			});
			$outerWrapper.css({
				'overflow': 'hidden'
			});
			var navigatorFunction = function(){
				$object.removeClass('mkslider-play');
				animateSlide($object.find('.mkslider-slide').index(
					$object.find('.mkslider-slide[data-mkslider-slider-start-number="' + $(this).attr('data-mkslider-navigator') + '"]')
				));
				return false;
			}
			if ($slides.length > 1){
				if (options.autoIcon){
					var $autoPlay = $('<div>').addClass('mkslider-auto-icon').addClass('mkslider-auto-icon-play').html(options.autoIconPlay).on('click',function(){
						$object.removeClass('mkslider-play');
					});
					var $autoPause = $('<div>').addClass('mkslider-auto-icon').addClass('mkslider-auto-icon-pause').html(options.autoIconPause).on('click',function(){
						$object.addClass('mkslider-play');
						if (!isAutoEnabled){
							auto();
						}
					});
					var $autoContainer = $('<div>').addClass('mkslider-auto').append($autoPlay).append($autoPause);
					$object.append($autoContainer);
				}
				if (options.arrows){
					var arrowFunction = function(){
						$object.removeClass('mkslider-play');
						animateSlide(parseInt($object.attr('data-mkslider-slide')) + parseInt($(this).attr('data-mkslider-arrow')));
						return false;
					}
					var $arrowNext = $('<div>').addClass('mkslider-arrow').addClass('mkslider-arrow-next').attr('data-mkslider-arrow',1).html(options.arrowNext).on('click',arrowFunction);
					var $arrowPrev = $('<div>').addClass('mkslider-arrow').addClass('mkslider-arrow-prev').attr('data-mkslider-arrow',-1).html(options.arrowPrev).on('click',arrowFunction);
					var $arrowsContainer = $('<div>').addClass('mkslider-arrows').append($arrowPrev).append($arrowNext);
					$object.append($arrowsContainer);
				}
				if (options.navigator){
					$navigator = $('<div>').addClass('mkslider-navigator');
					$object.append($navigator);
				}
				if (options.externalNavigator){
					$externalNavigator = $(options.externalNavigator);
				}
			}
			$slides.each(function(i){
				$(this).css({
					'width': (100 / $slides.length) + '%',
					'float': 'left'
				}).attr('data-mkslider-slider-start-number',i);
				if (options.navigator && ($slides.length > 1)){
					var $option = $('<div>').addClass('mkslider-navigator-item').attr('data-mkslider-navigator',i).html(options.navigatorItemText).on('click',navigatorFunction);
					if (i == 0){
						$option.addClass('mkslider-navigator-item-current');
					}
					$navigator.append($option);
				}
				if (options.externalNavigator && ($slides.length > 1)){
					$externalNavigator.find('> *').eq(i).addClass('mkslider-external-navigator-item').attr('data-mkslider-navigator',i).on('click',navigatorFunction);
					if (i == 0){
						$externalNavigator.find('> *').eq(i).addClass('mkslider-external-navigator-item-current');
					}
				}
			});
			isAutoEnabled = options.auto;
			if (options.auto){
				$object.addClass('mkslider-play');
				setTimeout(function(){
					auto();
				},options.autoTimeout)
			}
		};
		var auto = function(){
			if ($object.hasClass('mkslider-play')){
				isAutoEnabled = true;
				animateSlide(parseInt($object.attr('data-mkslider-slide')) + 1);
				setTimeout(function(){
					auto();
				},options.autoTimeout + options.animationTime)
			} else {
				isAutoEnabled = false;
			}
		}
		var animateSlide = function(n){
			var diff = Math.abs(parseInt($object.attr('data-mkslider-slide')) - n);
			if (n >= parseInt($object.attr('data-mkslider-slides'))){
				n = 1;
				$($object.find('.mkslider-slide:last')).insertBefore($object.find('.mkslider-slide:first'));
				$innerWrapper.css('left','0%');
				diff = 1;
			} else if (n < 0){
				n = 0;
				$($object.find('.mkslider-slide:last')).insertBefore($object.find('.mkslider-slide:first'));
				$innerWrapper.css('left','-100%');
				diff = 1;
			}
			$object.attr('data-mkslider-slide',n);
			if ($navigator){
				$navigator.find('.mkslider-navigator-item.mkslider-navigator-item-current').removeClass('mkslider-navigator-item-current');
			}
			if ($externalNavigator){
				$externalNavigator.find('.mkslider-external-navigator-item.mkslider-external-navigator-item-current').removeClass('mkslider-external-navigator-item-current');
			}
			$innerWrapper.stop().clearQueue().animate({
				'left': (-1 * n * 100) + '%'
			},options.animationPerSlide ? (diff * options.animationTime) : options.animationTime, function(){
				var ssn = parseInt($innerWrapper.find('.mkslider-slide').eq(n).attr('data-mkslider-slider-start-number'));
				if ($navigator){
					$navigator.find('.mkslider-navigator-item').eq(ssn).addClass('mkslider-navigator-item-current');
				}
				if ($externalNavigator){
					$externalNavigator.find('.mkslider-external-navigator-item').eq(ssn).addClass('mkslider-external-navigator-item-current');
				}
				var normalized = false;
				if ($innerWrapper.find('.mkslider-slide').eq(0).attr('data-mkslider-slider-start-number') != '0'){
					while (!normalized){
						if ($innerWrapper.find('.mkslider-slide').eq(0).attr('data-mkslider-slider-start-number') != '0'){
							$innerWrapper.find('.mkslider-slide').eq(0).insertAfter($innerWrapper.find('.mkslider-slide:last'));
						} else {
							normalized = true;
						}
					}
					var newN = (n == 0) ? ($innerWrapper.find('> *').length - 1) : 0;
					$innerWrapper.css('left',(newN * -100) + '%');
					$object.attr('data-mkslider-slide',newN);
				}
			});
		}
		return this.each(make);
	};
})(jQuery);