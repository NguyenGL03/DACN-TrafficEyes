import { ApplicationRef, ChangeDetectorRef, ElementRef, Injector } from "@angular/core";
import { ComponentBase } from "./component-base";
import moment from "moment";
export class ChangeDetectionComponent extends ComponentBase {
	cdr: ChangeDetectorRef;
	ref: ElementRef;
	appRef: ApplicationRef;

	constructor(injector: Injector) {
		super(injector);
		this.cdr = injector.get(ChangeDetectorRef);
		this.ref = injector.get(ElementRef);
		this.appRef = injector.get(ApplicationRef);
	}
	stopAutoUpdateView() {
		this.cdr.detach();
	}
	static encodeHTML(str) {
		return str.replace(/[\u00A0-\u9999<>&](?!#)/gim, function (i) {
			return '&#' + i.charCodeAt(0) + ';';
		});
	}

	static stopAllUpdateView: boolean;

	updateView(updateAll = true, stopAllUpdateView = false) {

		if (ChangeDetectionComponent.stopAllUpdateView) {
			return;
		}

		ChangeDetectionComponent.stopAllUpdateView = stopAllUpdateView;
		if (updateAll) {
			this.cdr.detectChanges();
		}
		ChangeDetectionComponent.stopAllUpdateView = false;
	}

	setupValidationMessage() {
		//NOTE - Comment do event khiến render liên tục

		// this.ref.nativeElement.querySelectorAll('input[required], textarea[required],input[pattern],money-input[required]>input,date-control>input, input[hidden][required]~input').forEach(x => {
		// 	var self = this;
		// 	x['focusout'] = function () {
		// 		if (self['isShowError']) {
		// 			self.updateView();
		// 		}
		// 	}
		// });

		// document.querySelectorAll('dropdown-control[required],all-code-select[required]').forEach(x => {
		// 	var self = this;
		// 	$(x).on('select2:select', function (e) {
		// 		if (self['isShowError']) {
		// 			self.updateView();
		// 		}
		// 	});
		// });
		// $("input[type=number]").on("keypress", function (event) {
		// 	// var keycode = evt.charCode || evt.keyCode;
		// 	var txtVal = $(this).val();
		// 	if (txtVal.toString().length == 15) {
		// 		event.preventDefault();
		// 		return false;
		// 	}
		// 	return true;
		// });

		// this.ref.nativeElement.querySelectorAll('input,textarea').forEach(x => {
		// 	x = x.parentElement;
		// 	x['isParent'] = true;
		// 	$(x).mouseenter(function () {
		// 		let value = $(this).find('input,textarea')[0] as any;
		// 		let val = value.value;
		// 		let oldData = $(value).attr('old-data');
		// 		if (oldData == val && $(".ui-tooltip").length > 0) {
		// 			return;
		// 		}

		// 		// $(".ui-tooltip").remove();
		// 		$(value).attr('old-data', val as any);
		// 		var textContent = '';
		// 		textContent = ChangeDetectionComponent.encodeHTML(val);

		// 		if (textContent.split(",").length > 1) {
		// 			var parsed = moment(textContent, "DD/MM/YYYY, hh:mm:ss a", true);
		// 			textContent = parsed.isValid() ? parsed.format("DD/MM/YYYY") : textContent;

		// 		} else {
		// 			var parsed = moment(textContent, "YYYY/MM/DD", true);

		// 			if (parsed.isValid()) {
		// 				console.log("YYYY/MM/DD     >>>    " + textContent);
		// 				var t = textContent = parsed.isValid() ? parsed.format("DD/MM/YYYY") : textContent;
		// 				console.log("Result     >>>    " + t);
		// 				console.log("Moment     >>>    ");
		// 				console.log(parsed)
		// 			}
		// 			textContent = parsed.isValid() ? parsed.format("DD/MM/YYYY") : textContent;
		// 		}

		// 		var colWidth = $(value).width();
		// 		// if (colWidth / textContent.length < 10) {
		// 		// 	$(this).tooltip({ disabled: true })
		// 		// 	$(this).tooltip({
		// 		// 		disabled: false,
		// 		// 		items: "div, input, textarea",
		// 		// 		content: textContent,
		// 		// 		close: function (event, ui) {
		// 		// 			$(".ui-tooltip").remove();
		// 		// 		},
		// 		// 		hide: false,
		// 		// 		tooltipClass: "custom_tooltip"
		// 		// 	});
		// 		// }
		// 		// else {
		// 		// 	$(this).tooltip({ disabled: true })
		// 		// }
		// 	})


		// 	$(x).mouseleave(function () {
		// 		$(".ui-tooltip").remove();
		// 		// $(this).tooltip({ disabled: true })
		// 	});
		// })
	}
	updateParentView() {
		var par = this.cdr['_view'].parent;
		if (par && par.component && par.component.updateView) {
			par.component.updateView();
		}
	}

}

