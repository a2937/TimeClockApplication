// Write your JavaScript code.
function setCheckedBoxValue(checkBoxElementId)
{
    var checkbox = document.getElementById(checkBoxElementId);
    if (checkbox.checked)
    {
        checkbox.value = true;
    }
    else
    {
        checkbox.value = false;
    }
}