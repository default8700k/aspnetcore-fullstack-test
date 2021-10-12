let fieldsCount = 0;
$(document).ready(function() {
	let $containerFields = $('div[id="fields"]');
	let $templateField = $('template[id="field"]');

	let $btnFieldAdd = $('button[id="btn-field-add"]');
	let $btnFieldRemove = $('button[id="btn-field-remove"]');
	$btnFieldRemove.attr('disabled', true);

	$btnFieldAdd.click(function() {
		let templateField = $templateField.html().replaceAll('$.id', fieldsCount + 1);
		$containerFields.append(templateField);

		fieldsCount++;
		if (fieldsCount > 1 && $btnFieldRemove.attr('disabled') !== undefined) {
			$btnFieldRemove.removeAttr('disabled');
		}
	});
	$btnFieldAdd.trigger('click');

	$btnFieldRemove.click(function() {
		if (fieldsCount <= 1) {
			return console.warn('warning: fieldsCount less than 1');
		}

		$containerFields.find('input[id^="field"]').last()
			.parent().remove();

		fieldsCount--;
		if (fieldsCount <= 1 && $btnFieldRemove.attr('disabled') === undefined) {
			$btnFieldRemove.attr('disabled', true);
		}
	});

	let $btnCalculate = $('button[id="btn-calculate"]');
	$btnCalculate.click(function() {
		console.log('calculate');
	});
});