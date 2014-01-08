/*!
 * jQuery twitter bootstrap wizard plugin
 * Examples and documentation at: http://github.com/VinceG/twitter-bootstrap-wizard
 * version 1.0
 * Requires jQuery v1.3.2 or later
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 * Authors: Vadim Vincent Gabriel (http://vadimg.com), Jason Gill (www.gilluminate.com)
 */
;(function($) {
var bootstrapWizardCreate = function(element, options) {
	var element = $(element);
	var obj = this;

	// Merge options with defaults
	var $settings = $.extend({}, $.fn.bootstrapWizard.defaults, options);
	var $activeTab = null;
	var $navigation = null;
	
	this.rebindClick = function(selector, fn)
	{
		selector.unbind('click', fn).bind('click', fn);
	}

	this.fixNavigationButtons = function() {
//	log("Fixing Buttons");
		// Get the current active tab
		if(!$activeTab.length) {
			// Select first one
			$navigation.find('a:first').tab('show');
			$activeTab = $navigation.find('a:first');
//            log("calculated active tab");
		}

//        log("Active tab length " + $activeTab.length);

		// See if we're currently in the first/last then disable the previous and last buttons
		$($settings.previousSelector, element).toggleClass('disabled', (obj.firstIndex() >= obj.currentIndex()));
		$($settings.nextSelector, element).toggleClass('disabled', (obj.currentIndex() >= obj.navigationLength()));

		// We are unbinding and rebinding to ensure single firing and no double-click errors
		obj.rebindClick($($settings.nextSelector, element), obj.next);
		obj.rebindClick($($settings.previousSelector, element), obj.previous);
		obj.rebindClick($($settings.lastSelector, element), obj.last);
		obj.rebindClick($($settings.firstSelector, element), obj.first);
//		log("Finished Fixing Buttons");
		if($settings.onTabShow && typeof $settings.onTabShow === 'function' && $settings.onTabShow($activeTab, $navigation, obj.currentIndex())===false){
			return false;
		}
	};

	this.next = function(e) {
//	log("clicked next ");
		// If we clicked the last then dont activate this
		if(element.hasClass('last')) {
			return false;
		}

		if($settings.onNext && typeof $settings.onNext === 'function' && $settings.onNext($activeTab, $navigation, obj.nextIndex())===false){
			return false;
		}

		// Did we click the last button
		$index = obj.nextIndex();
		if($index > obj.navigationLength()) {
		} else {
//            log("nextNav: " + ($navigation.find('a:eq('+$index+')').prop('tagName')));
			$navigation.find('a:eq('+$index+')').tab('show');
            obj.showNextTab();
		}
	};

	this.previous = function(e) {

		// If we clicked the first then dont activate this
		if(element.hasClass('first')) {
			return false;
		}

		if($settings.onPrevious && typeof $settings.onPrevious === 'function' && $settings.onPrevious($activeTab, $navigation, obj.previousIndex())===false){
			return false;
		}

		$index = obj.previousIndex();
		if($index < 0) {
		} else {
			$navigation.find('a:eq('+$index+')').tab('show');
            obj.showPreviousTab();
		}
	};

	this.first = function(e) {
		if($settings.onFirst && typeof $settings.onFirst === 'function' && $settings.onFirst($activeTab, $navigation, obj.firstIndex())===false){
			return false;
		}

		// If the element is disabled then we won't do anything
		if(element.hasClass('disabled')) {
			return false;
		}
		$navigation.find('a:eq(0)').tab('show');

	};
	this.last = function(e) {
		if($settings.onLast && typeof $settings.onLast === 'function' && $settings.onLast($activeTab, $navigation, obj.lastIndex())===false){
			return false;
		}

		// If the element is disabled then we won't do anything
		if(element.hasClass('disabled')) {
			return false;
		}
		$navigation.find('a:eq('+obj.navigationLength()+')').tab('show');
	};
	this.currentIndex = function() {
//	log("currentIndex " + $navigation.find('a').index($activeTab));
		return $navigation.find('a').index($activeTab);
	};
	this.firstIndex = function() {
		return 0;
	};
	this.lastIndex = function() {
		return obj.navigationLength();
	};
	this.getIndex = function(e) {
		return $navigation.find('a').index(e);
	};
	this.nextIndex = function() {
//    log("Active Tab = " + $activeTab.html());
//	log("nextIndex " + $navigation.find('a').index($activeTab) + 1);
		return $navigation.find('a').index($activeTab) + 1;
	};
	this.previousIndex = function() {
		return $navigation.find('a').index($activeTab) - 1;
	};
	this.navigationLength = function() {
//        log("Calculating Navigation Length " + ($navigation.find('li').length -1));
		return $navigation.find('a').length - 1;
	};
	this.activeTab = function() {
//	log("Active Tab " + $activeTab);
		return $activeTab;
	};
	this.nextTab = function() {
		return $navigation.find('a:eq('+(obj.currentIndex()+1)+')').length ? $navigation.find('a:eq('+(obj.currentIndex()+1)+')') : null;
	};
	this.previousTab = function() {
		if(obj.currentIndex() <= 0) {
			return null;
		}
		return $navigation.find('a:eq('+parseInt(obj.currentIndex()-1)+')');
	};
	this.show = function(index) {
		return element.find('a:eq(' + index + ')').tab('show');
	};
	this.disable = function(index) {
//		log("disabling " + index);
		$navigation.find('a:eq('+index+')').addClass('disabled');
	};
	this.enable = function(index) {
//	log("enabling " + index);
		$navigation.find('a:eq('+index+')').removeClass('disabled');
	};
	this.hide = function(index) {
		$navigation.find('a:eq('+index+')').hide();
	};
	this.display = function(index) {
		$navigation.find('a:eq('+index+')').show();
	};
	this.remove = function(args) {
		var $index = args[0];
		var $removeTabPane = typeof args[1] != 'undefined' ? args[1] : false;
		var $item = $navigation.find('a:eq('+$index+')');

		// Remove the tab pane first if needed
		if($removeTabPane) {
			var $href = $item.find('a').attr('href');
			$($href).remove();
		}

		// Remove menu item
		$item.remove();
	};

    
    this.showNextTab = function() {
//            log("NextTab calculated");

            $activeTab.find('li').first().removeClass('active');
            $activeTab.find('span').first().removeClass('badge-info');

            $activeTab.find('li').first().addClass('complete');
            $activeTab.find('span').first().addClass('badge-success');

		    $activeTab = obj.nextTab() 

            $activeTab.find('li').first().addClass('active');
            $activeTab.find('span').first().addClass('badge-info');

		    obj.fixNavigationButtons();
    };

    this.showPreviousTab = function() {

            $activeTab.find('li').first().toggleClass('active');
            $activeTab.find('span').first().toggleClass('badge-info');

            $activeTab.find('li').first().addClass('complete');
            $activeTab.find('span').first().addClass('badge-success');

		    $activeTab = obj.previousTab(); 

            $activeTab.find('li').first().toggleClass('active');
            $activeTab.find('span').first().toggleClass('badge-info');

		    obj.fixNavigationButtons();
    };

	$navigation = element.find('ul:first', element);
	$activeTab = $navigation.find('a.active', element);

    $activeTab.tab('show');

	if(!$navigation.hasClass($settings.tabClass)) {
		$navigation.addClass($settings.tabClass);
	}

	// Load onInit
	if($settings.onInit && typeof $settings.onInit === 'function'){
		$settings.onInit($activeTab, $navigation, 0);
	}

	// Load onShow
	if($settings.onShow && typeof $settings.onShow === 'function'){
		$settings.onShow($activeTab, $navigation, obj.nextIndex());
	}

	// Work the next/previous buttons
	obj.fixNavigationButtons();

	$('a[data-toggle="tab"]', $navigation).on('click', function (e) {
		// Get the index of the clicked tab
//        log("Anchor Toggle on click");
		var clickedIndex = $navigation.find('a').index($(e.currentTarget));
//        log("Clicked " + clickedIndex + $activeTab.html());
        if(clickedIndex == obj.currentIndex()){
            return false;
        }

		if($settings.onTabClick && typeof $settings.onTabClick === 'function' && $settings.onTabClick($activeTab, $navigation, obj.currentIndex(), clickedIndex)===false){
			return false;
		}
        $activeTab.find('li').first().removeClass('active');
        $activeTab.find('span').first().removeClass('badge-info');

        $activeTab.find('li').first().addClass('complete');
        $activeTab.find('span').first().addClass('badge-success');

        $(e.target).addClass('active');
        $(e.target).find('span').first().addClass('badge-info');

        $activeTab = $navigation.find('a:eq('+clickedIndex+')')
//        log("New active Tab " + $activeTab.html());
        obj.fixNavigationButtons();
	});

	$('a[data-toggle="tab"]', $navigation).on('shown', function (e) {  // use shown instead of show to help prevent double firing

//    log("Anchor Toggle on shown");
		$element = $(e.target).parent();
		var nextTab = $navigation.find('a').index($element);

		// If it's disabled then do not change
		if($element.hasClass('disabled')) {
			return false;
		}

		if($settings.onTabChange && typeof $settings.onTabChange === 'function' && $settings.onTabChange($activeTab, $navigation, obj.currentIndex(), nextTab)===false){
				return false;
		}

		$activeTab = $element; // activated tab
		obj.fixNavigationButtons();
	});
};




$.fn.bootstrapWizard = function(options) {
	//expose methods
	if (typeof options == 'string') {
		var args = Array.prototype.slice.call(arguments, 1)
		if(args.length === 1) {
			args.toString();
		}
		return this.data('bootstrapWizard')[options](args);
	}
	return this.each(function(index){
		var element = $(this);
		// Return early if this element already has a plugin instance
		if (element.data('bootstrapWizard')) return;
		// pass options to plugin constructor
		var wizard = new bootstrapWizardCreate(element, options);
		// Store plugin object in this element's data
		element.data('bootstrapWizard', wizard);
	});
};




// expose options
$.fn.bootstrapWizard.defaults = {
	tabClass:         'nav nav-pills',
	nextSelector:     'li.next',
	previousSelector: 'li.previous',
	firstSelector:    'li.first',
	lastSelector:     'li.last',
	onShow:           null,
	onInit:           null,
	onNext:           null,
	onPrevious:       null,
	onLast:           null,
	onFirst:          null,
	onTabChange:      null, 
	onTabClick:       null,
	onTabShow:        null
};

})(jQuery);



//function log(msg) {
//	setTimeout(function() {
//		throw new Error("Log: " + msg);
//	}, 0);
//};