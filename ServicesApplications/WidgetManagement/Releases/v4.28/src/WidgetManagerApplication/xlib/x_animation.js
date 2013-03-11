/* This is currently under development. All of this will probably change! mf, 25Sep06 */

// xAnimation, Copyright 2006 Michael Foster (Cross-Browser.com)
// Part of X, a Cross-Browser Javascript Library, Distributed under the terms of the GNU LGPL

// xAnimation, xAniLine, xAniOpacity, xAniEllipse, xAniScroll, xAniSize

/*
Other properties of xAnimation:
  x,y // instantaneous point
  af // acceleration factor
  ap // acceleration period
  t1 // start time
  et // elapsed time
  tmr // timer handle
  x1,y1 // start point or angle
  x2,y2 // end point or angle
  xm,ym // component magnitudes or arc length
  as // auto-start
Other properties used only by callbacks:
 xEllipse:
  xr,yr // x and y radii
  xc,yc // arc center point
 xSlideCorner:
  corner // corner string
 xParaEq:
  xs,ys // expression strings
  inc // T increment
*/

function xAnimation(e, at, qc, tt, orf, otf, oed, oea, oef) // Object Prototype
{
  this.init(e, at, qc, tt, orf, otf, oed, oea, oef);
  var a = xAnimation.instances;
  var i;
  for (i = 0; i < a.length; ++i) { if (!a[i]) break; } // find an empty slot
  a[i] = this;
  this.idx = i;
}

xAnimation.instances = []; // static property

xAnimation.prototype.init = function(e, at, qc, tt, orf, otf, oed, oea, oef)
{
  this.e = xGetElementById(e); // e is object ref or id str
  this.at = at || 2; // acceleration type: 1=linear, 2=sine, 3=cosine
  this.qc = qc || 1; // acceleration quarter-cycles
  this.tt = tt; // total time
  this.orf = orf; // onRun function
  this.otf = otf; // onTarget function
  this.oed = oed; // delay before calling onEnd
  this.oea = oea; // onEnd argument
  this.oef = oef; // onEnd function
  this.to = 20; // timer timeout
  this.as = true; // auto-start
  return this;
};

xAnimation.prototype.start = function()
{
  var a = this;
  // acceleration
  if (a.at == 1) { a.ap = 1 / a.tt; } // linear
  else { a.ap = a.qc * (Math.PI / (2 * a.tt)); } // sine and cosine
  // magnitudes
  if (xDef(a.x1)) { a.xm = a.x2 - a.x1; }
  if (xDef(a.y1)) { a.ym = a.y2 - a.y1; }
  // end point if even number of cyles
  if (!(a.qc % 2)) {
    if (xDef(a.x1)) a.x2 = a.x1;
    if (xDef(a.y1)) a.y2 = a.y1;
  }
  if (!a.tmr) { // if not already running
    var d = new Date();
    a.t1 = d.getTime(); // start time
    a.tmr = setTimeout('xAnimation.run(' + a.idx + ')', 1);
  }
};

xAnimation.run = function(i) // static method
{
  var a = xAnimation.instances[i];
  if (!a) { /*alert('invalid index');*/ return; }
  var d = new Date();
  a.et = d.getTime() - a.t1; // elapsed time
  if (a.et < a.tt) {
    a.tmr = setTimeout('xAnimation.run(' + i + ')', a.to);
    // acceleration
    a.af = a.ap * a.et;  // linear
    if (a.at == 2) { a.af = Math.abs(Math.sin(a.af)); } // sine
    else if (a.at == 3) { a.af = 1 - Math.abs(Math.cos(a.af)); } // cosine
    // instantaneous point
    if (xDef(a.x1)) a.x = a.xm * a.af + a.x1;
    if (xDef(a.y1)) a.y = a.ym * a.af + a.y1;
    a.orf(a); // onRun
  }
  else {
    var rep = false; // repeat
    if (xDef(a.x2)) a.x = a.x2;
    if (xDef(a.y2)) a.y = a.y2;
    a.tmr = null;
    a.otf(a); // onTarget
    if (xDef(a.oef)) { // onEnd
      if (a.oed) setTimeout(a.oef, a.oed); // no args passed to oef if oed or oef is str
      else if (xStr(a.oef)) { rep = eval(a.oef); }
      else { rep = a.oef(a, a.oea); }
    }
    if (rep) { a.resume(true); } // there may be a problem here if the anim is paused and oef returns true
/*
    if (rep && a.tmr) {
      a.tmr = null;
      a.resume(true);
    }
    else a.tmr = null;
*/
  }
};

xAnimation.prototype.pause = function()
{
  var s = false;
  if (this.tmr) {
    clearTimeout(this.tmr);
    this.tmr = null;
    s = true;
  }
  return s;
};

xAnimation.prototype.resume = function(fromStart)
{
  var s = false;
  if (!this.tmr) {
    var d = new Date();
    this.t1 = d.getTime();
    if (!fromStart) this.t1 -= this.et; // new start time is current time minus elapsed time
    this.tmr = setTimeout('xAnimation.run(' + this.idx + ')', this.to);
    s = true;
  }
  return s;
};

xAnimation.prototype.kill = function()
{
  this.pause();
  xAnimation.instances[this.idx] = null;
};

// end xAnimation

//---------------------------------------------------------------------------
// xAniLine - Animate an element over a line segment.

function xAniLine(xa, x, y, tt, at, qc, oed, oea, oef)
{
  var a = xa.init(xa.e, at, qc, tt, onRun, onRun, oed, oea, oef);
  a.x1 = xLeft(a.e); // start point
  a.y1 = xTop(a.e);
  a.x2 = x; // end point
  a.y2 = y;
  if (a.as) a.start();
  return a;
  function onRun(o) { xMoveTo(o.e, Math.round(o.x), Math.round(o.y)); }
}

//---------------------------------------------------------------------------
// xAniOpacity - Animate an element's opacity.

function xAniOpacity(xa, o, tt, at, qc, oed, oea, oef)
{
  var a = xa.init(xa.e, at, qc, tt, onRun, onRun, oed, oea, oef);
  a.x1 = xOpacity(a.e); // start opacity
  a.x2 = o; // end opacity
  if (a.as) a.start();
  return a;
  function onRun(o) { xOpacity(o.e, o.x); }
  // (o.x == 1) ? 0.9999 : o.x; // for Gecko bug?
}

//---------------------------------------------------------------------------
// xAniEllipse - Animate an element over an elliptical arc.

function xAniEllipse(xa, xr, yr, a1, a2, tt, at, qc, oed, oea, oef)
{
  var a = xa.init(xa.e, at, qc, tt, onRun, onRun, oed, oea, oef);
  a.x1 = a1 * (Math.PI / 180); // start angle
  a.x2 = a2 * (Math.PI / 180); // end angle
  var sx = xLeft(a.e) + (xWidth(a.e) / 2); // start point
  var sy = xTop(a.e) + (xHeight(a.e) / 2);
  a.xc = sx - (xr * Math.cos(a.x1)); // arc center point
  a.yc = sy - (yr * Math.sin(a.x1));
  a.xr = xr; // radii
  a.yr = yr;
  if (a.as) a.start();
  return a;
  function onRun(o) {
    xMoveTo(o.e,
      Math.round(o.xr * Math.cos(o.x)) + o.xc - (xWidth(o.e) / 2),
      Math.round(o.yr * Math.sin(o.x)) + o.yc - (xHeight(o.e) / 2));
  }
}

//---------------------------------------------------------------------------
// xAniScroll - animate the scroll position of a window or element.

function xAniScroll(xa, x, y, tt, at, oed, oea, oef)
{
  var a = xa.init(xa.e, at, 1, tt, onRun, onRun, oed, oea, oef);
  a.x1 = xScrollLeft(a.e, 1); // start point
  a.y1 = xScrollTop(a.e, 1);
  a.x2 = x; // end point
  a.y2 = y;
  if (a.as && a.e.scrollTo) a.start();
  //else alert('scrollTo not supported');
  return a;
  function onRun(o) { o.e.scrollTo(Math.round(o.x), Math.round(o.y)); }
}

//---------------------------------------------------------------------------
// xAniSize - Animate an element's size.

function xAniSize(xa, w, h, tt, at, qc, oed, oea, oef)
{
  var a = xa.init(xa.e, at, qc, tt, onRun, onRun, oed, oea, oef);
  a.x1 = xWidth(a.e); // start size
  a.y1 = xHeight(a.e);
  a.x2 = w; // end size
  a.y2 = h;
  if (a.as) a.start();
  return a;
  function onRun(o) { xResizeTo(o.e, Math.round(o.x), Math.round(o.y)); }
}

//---------------------------------------------------------------------------
// * not yet tested *
// xAniCorner - Animate an element's corner.

function xAniCorner(xa, corner, x, y, tt, at, qc, oed, oea, oef)
{
  var a = xa.init(xa.e, at, qc, tt, onRun, onRun, oed, oea, oef);
  var e = a.e, ex = xLeft(e), ey = xTop(e);
  a.x2 = x; // end point
  a.y2 = y;
  // start point
  a.corner = corner.toLowerCase();
  switch (a.corner) {
    case 'nw': a.x1 = ex; a.y1 = ey; break;
    case 'sw': a.x1 = ex; a.y1 = ey + xHeight(e); break;
    case 'ne': a.x1 = ex + xWidth(e); a.y1 = ey; break;
    case 'se': a.x1 = ex + xWidth(e); a.y1 = ey + xHeight(e); break;
    default: /*alert('invalid corner');*/ return;
  }
  if (a.as) a.start();
  return a;
  function onRun(o) {
    var e = o.e, x = o.x, y = o.y;
    var ex = xLeft(e), ey = xTop(e); // nw point
    var sex = ex + xWidth(e); // se point
    var sey = ey + xHeight(e);
    switch (o.corner) {
      case 'nw': xMoveTo(e, x, y); xResizeTo(e, sex - ex, sey - ey); break;
      case 'sw': if (o.x2 != ex) { xLeft(e, x); xWidth(e, sex - ex); } xHeight(e, y - ey); break;
      case 'ne': xWidth(e, x - ex); if (o.y2 != ey) { xTop(e, y); xHeight(e, sey - ey); } break;
      case 'se': xWidth(e, x - ex); xHeight(e, y - ey); break;
    }
  }
}

//---------------------------------------------------------------------------
// * not yet tested *
// xAniParaEq - Animate an element over parametric equations.

function xAniParaEq(xa, xs, ys, tt, inc)
{
  var a = xa.init(xa.e, 1, 1, tt || 5000, onRun, onRun, 0, null, !tt ? 'true' : null);
  a.xs = xs; a.ys = ys; // x and y expression strings
  a.inc = inc || .008; // T increments by this on each iteration
  window.T = 0; // T is global!!! Yuck!!!
  if (a.as) a.start();
  return a;
  function onRun(o) {
    var p = xParent(o.e), xc, yc;
    xc = (xWidth(p)/2)-(xWidth(e)/2); // center of parent
    yc = (xHeight(p)/2)-(xHeight(e)/2);
    window.T += o.inc;
    xMoveTo(o.e,
      Math.round((eval(o.xs) * xc) + xc) + xScrollLeft(p),
      Math.round((eval(o.ys) * yc) + yc) + xScrollTop(p));
  }
}

//---------------------------------------------------------------------------

function dbgMsg(s)
{
  var dbg = xGetElementById('dbgMsgEle');
  if (dbg) {
    xDisplay(dbg, 'block');
    dbg.innerHTML += s;
  }
}

/*
  function onRun(o) {
    msg('X: ' + o.xm + ' * ' + o.af + ' + ' + o.x1 + ' = ' + (o.xm * o.af + o.x1));
    msg('Y: ' + o.ym + ' * ' + o.af + ' + ' + o.y1 + ' = ' + (o.ym * o.af + o.y1));
    msg('');
  }
  function dbg_onRun(o) {
    //msg("<div class='m0'><div class='m1'>A:<\/div><div class='m2'>" + o.ep + "<\/div><div class='m3'>*<\/div><div class='m4'>" + o.af + "<\/div><div class='m5'>+<\/div><div class='m6'>" + o.a1 + "<\/div><div class='m7'>=<\/div><div class='m8'>" + (o.ep * o.af + o.a1) + "<\/div><\/div>");
    msg("<div class='m0'><div class='m1'>X:<\/div><div class='m2'>" + o.xm + "<\/div><div class='m3'>*<\/div><div class='m4'>" + o.af + "<\/div><div class='m5'>+<\/div><div class='m6'>" + o.x1 + "<\/div><div class='m7'>=<\/div><div class='m8'>" + (o.xm * o.af + o.x1) + "<\/div><\/div>");
    //msg("<div class='m0'><div class='m1'>Y:<\/div><div class='m2'>" + o.ym + "<\/div><div class='m3'>*<\/div><div class='m4'>" + o.af + "<\/div><div class='m5'>+<\/div><div class='m6'>" + o.y1 + "<\/div><div class='m7'>=<\/div><div class='m8'>" + (o.ym * o.af + o.y1) + "<\/div><\/div>");
    //msg('Y: ' + o.ym + ' * ' + o.af + ' + ' + o.y1 + ' = ' + (o.ym * o.af + o.y1)) + '<br>';
  }
*/

// just for testing offline
function xaMenu()
{
  document.write(
    "<p>This is a preview of xAnimation and it's <i>friend</i> functions. This is currently just an experiment! (26Sep06).</p>"
    + "<p><a href='xanimation.html'>xAnimation</a> &nbsp;|&nbsp;"
    + " <a href='xaniscroll.html'>xAniScroll</a> &nbsp;|&nbsp;"
    + " <a href='xaniline.html'>xAniLine</a> &nbsp;|&nbsp;"
    + " <a href='xaniellipse.html'>xAniEllipse</a> &nbsp;|&nbsp;"
    + " <a href='xaniopacity.html'>xAniOpacity</a> &nbsp;|&nbsp;"
    + " <a href='xanisize.html'>xAniSize</a> &nbsp;|&nbsp;"
    + " more to come..."
//    + " <a href='xanicorner.html'>xAniCorner</a> &nbsp;|&nbsp;"
//    + " <a href='xaniparaeq.html'>xAniParaEq</a>"
    + "</p>"
  );
}

