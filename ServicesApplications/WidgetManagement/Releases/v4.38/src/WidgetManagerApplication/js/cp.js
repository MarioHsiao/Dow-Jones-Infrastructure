// Colour Picker v1.0
// Provides functionality for a dropdown palette of colours.
// Copyright (C) 2006 Neil Fraser
// http://neil.fraser.name/software/colourpicker/

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License (www.gnu.org) for more details.


// Include at the top of your page:
//   <SCRIPT LANGUAGE='JavaScript1.3' SRC='cp.js'></SCRIPT>
// Call with:
//   <INPUT NAME="mycolour" ID="mycolour" VALUE="ff00ff">
//   <SCRIPT LANGUAGE='JavaScript1.3'><!--
//   cp_init("mycolour")
//   //--></SCRIPT>

var cp_grid = [
  ["ffffff", "ffcccc", "ffcc99", "ffff99", "ffffcc", "99ff99", "99ffff", "ccffff", "ccccff", "ffccff"],
  ["cccccc", "ff6666", "ff9966", "ffff66", "ffff33", "66ff99", "33ffff", "66ffff", "9999ff", "ff99ff"],
  ["c0c0c0", "ff0000", "ff9900", "ffcc66", "ffff00", "33ff33", "66cccc", "33ccff", "6666cc", "cc66cc"],
  ["999999", "cc0000", "ff6600", "ffcc33", "ffcc00", "33cc00", "00cccc", "3366ff", "6633ff", "cc33cc"],
  ["666666", "990000", "cc6600", "cc9933", "999900", "009900", "339999", "3333ff", "6600cc", "993399"],
  ["333333", "660000", "993300", "996633", "666600", "006600", "336666", "000099", "333399", "663366"],
  ["000000", "330000", "663300", "663333", "333300", "003300", "003333", "000066", "330099", "330033"]];

var cp_dom = null;
var cp_caller = null;
var cp_defaultcolour = 'ffffff'
var cp_closePID = null;

function cp_init(id) {
  // Hide the form element, and replace it with a colour box.
  var obj = document.getElementById(id);
  if (!obj) {
    alert("Colour picker can't find '"+id+"'");
    return;
  }
  if (!cp_hex2rgb(obj.value)) {
    //alert("Colour picker can't parse colour code in '"+id+"'");
    obj.value = "99CC00";
  }
  // IE 6 chokes on this:
  //   obj.type = "hidden";
  // So we'll do it the hard way:
  var newobj = document.createElement("input");
  newobj.type = "hidden"
  newobj.value = obj.value;
  newobj.name = obj.name;
  if (obj.onchange) newobj.onchange = obj.onchange;
  obj.parentNode.insertBefore(newobj, obj);
  obj.parentNode.removeChild(obj);
  newobj.id = id;
  obj = newobj;
  
  // <IMG style="background-color: #ff0000; border: outset 3px #888;" SRC="1x1.gif" HEIGHT=20 WIDTH=30>
  var box = document.createElement("img");
  box.style.backgroundColor = "#"+obj.value;
  box.style.border = "solid 1px #888";
  box.src = "../img/1x1.gif";
  box.height = 16;
  box.width = 16;
  box.label = id;
  box.onclick = new Function("cp_open(this)");
  box.onmouseover = cp_cancelclose;
  box.onmouseout = cp_closesoon;
  obj.parentNode.insertBefore(box, obj);
}

function cp_open(caller) {
  // Create a table of colours.
  if (cp_dom) {
    cp_close();
    return;
  }
  cp_caller = caller;
//  alert(document.getElementById(caller.label));
  cp_defaultcolour = document.getElementById(caller.label).value;
  var posX = 0;
  var posY = caller.offsetHeight;
  while (caller) {
    posX += caller.offsetLeft;
    posY += caller.offsetTop;
    caller = caller.offsetParent;
  }
  cp_dom = document.createElement("div");
  var table = document.createElement("table");
  table.setAttribute("border", "1");
  table.style.backgroundColor = "#808080";
  table.onmouseover = cp_cancelclose;
  table.onmouseout = cp_closesoon;
  var tbody = document.createElement("tbody"); // IE 6 needs this.
  var row, cell;
  for (var y=0; y<cp_grid.length; y++) {
    row = document.createElement("tr");
    tbody.appendChild(row);
    for (var x=0; x<cp_grid[y].length; x++) {
      cell = document.createElement("td");
      row.appendChild(cell);
      cell.style.backgroundColor = "#"+cp_grid[y][x];
      cell.label = cp_grid[y][x];
      cell.style.border = "solid 2px #"+cell.label;
      cell.onmouseover = cp_onmouseover;
      cell.onmouseout = cp_onmouseout;
      cell.onclick = cp_onclick;
      cell.innerHTML = "<IMG SRC='../img/1x1.gif' HEIGHT=16 WIDTH=16>";
      if (cp_defaultcolour.toLowerCase() == cp_grid[y][x].toLowerCase()) {
        cell.onmouseover();
        cell.onmouseout();
      }
    }
  }
  table.appendChild(tbody);
  cp_dom.appendChild(table);

  cp_dom.style.position = "absolute";
  cp_dom.style.left = "0px";
  cp_dom.style.top = "0px";
  cp_dom.style.visibility = "hidden";
  document.body.appendChild(cp_dom);
  // Don't widen the screen.
  if (posX + cp_dom.offsetWidth > document.body.offsetWidth)
    posX = document.body.offsetWidth - cp_dom.offsetWidth;
  cp_dom.style.left = posX+"px";
  cp_dom.style.top = posY+"px";
  cp_dom.style.visibility = "visible";
}

function cp_close() {
  // Close the table now.
  cp_cancelclose();
  if (cp_dom)
    document.body.removeChild(cp_dom)
  cp_dom = null;
  cp_caller = null;
}

function cp_closesoon() {
  // Close the table a split-second from now.
  cp_closePID = window.setTimeout("cp_close()", 250);
}

function cp_cancelclose() {
  // Don't close the colour table after all.
  if (cp_closePID)
    window.clearTimeout(cp_closePID);
}

function cp_onclick() {
  // Clicked on a colour.  Close the table, set the colour, fire an onchange event.
  cp_caller.style.backgroundColor = "#"+this.label;
  var input = document.getElementById(cp_caller.label)
  input.value = this.label;
  cp_close();
  if (input.onchange)
    input.onchange();
}

function cp_onmouseover() {
  // Place a black border on the cell if the contents are light, a white border if the contents are dark.
  this.style.borderStyle = "dotted";
  var rgb = cp_hex2rgb(this.label);
  if (rgb[0]+rgb[1]+rgb[2] > 255*3/2)
    this.style.borderColor = "black";
  else
    this.style.borderColor = "white";
}

function cp_onmouseout() {
  // Remove the border.
  if (this.label == cp_defaultcolour)
    this.style.borderStyle = "outset";
  else
    this.style.border = "solid 2px #"+this.label;
}

function cp_hex2rgb(hexcode) {
  // Parse '0088ff' and return the [r, g, b] ints.
  var m = hexcode.match(/^([a-f0-9][a-f0-9])([a-f0-9][a-f0-9])([a-f0-9][a-f0-9])$/);
  if (m) {
    var r = parseInt(m[1], 16);
    var g = parseInt(m[2], 16);
    var b = parseInt(m[3], 16);
    return [r, g, b];
  } else {
    return null;
  }
}
