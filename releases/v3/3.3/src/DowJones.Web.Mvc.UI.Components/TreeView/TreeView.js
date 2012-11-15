DJ.UI.TreeView = DJ.UI.Component.extend({
    defaults: {
        enableCheckboxes: false,
        expandAll: false
    },

    events: {
    },

    init: function (element, meta) {
		this.classNames = {
			treeContainer: "dj_tree_view",
			checked: "dj_tree_view_checked",
			unchecked: "dj_tree_view_unchecked",
			undetermined: "dj_tree_view_undetermined",
			leaf: "dj_tree_view_leaf",
			open: "dj_tree_view_open",
			closed: "dj_tree_view_closed",
			toggle: "dj_tree_view_node_toggle"
		};
		
		this.selectors = {
			node: ".dj_tree_view_node",
			nodeContent: ".dj_tree_view_node_content",
			checkbox: ".dj_tree_view_node_checkbox",
			text: ".dj_tree_view_node_text",
			leaf: "." + this.classNames.leaf,
			open: "." + this.classNames.open,
			closed: "." + this.classNames.closed,
			treeContainer: "." + this.classNames.treeContainer,
			toggle: "." + this.classNames.toggle
		};
		
        var $meta = $.extend({ name: "TreeView" }, meta);

        // Call the base constructor
        this._super(element, $meta);
    },

    _initializeElements: function () {
    },

    _initializeEventHandlers: function () {
        var $parentContainer = this.$element
                , me = this;

        this.$element
			.delegate(this.selectors.checkbox, 'click', function() {
				var $node = $(this).closest(me.selectors.node);
				
				if ($node.hasClass(me.classNames.unchecked)) {
					me._setCheckedClass($node, me.classNames.checked, true);
				}
				else {
					me._setCheckedClass($node, me.classNames.unchecked, true);
				}
				return false;
			})
			.delegate(this.selectors.toggle, "click", function() {
				var $node = $(this).closest(me.selectors.node);
				if ($node.hasClass(me.classNames.leaf)) {
					// do nothing
					return;
				}
				else if ($node.hasClass(me.classNames.open)) {
					$node.removeClass(me.classNames.open).addClass(me.classNames.closed);
				}
				else if ($node.hasClass(me.classNames.closed)) {
					$node.removeClass(me.classNames.closed).addClass(me.classNames.open);
				}
			});
    },
	
	_setCheckedClass: function($node, className, setChildren) {
		(setChildren ? $node.find(this.selectors.node).andSelf() : $node)
			.removeClass(this.classNames.checked)
			.removeClass(this.classNames.unchecked)
			.removeClass(this.classNames.undetermined)
			.addClass(className);
		
		// update parent nodes
		var $parentNodes = $node.parents(this.selectors.node);
		if ($parentNodes.length > 0) {
			var $children = $node.siblings().andSelf();
			var checkedChildrenCount = $children.filter("." + this.classNames.checked).length;
			if (checkedChildrenCount === 0) {
				if ($children.filter("." + this.classNames.undetermined).length > 0)
					this._setCheckedClass($parentNodes.first(), this.classNames.undetermined, false);
				else
					this._setCheckedClass($parentNodes.first(), this.classNames.unchecked, false);
			}
			else if (checkedChildrenCount < $children.length) {
				this._setCheckedClass($parentNodes.first(), this.classNames.undetermined, false);
			}
			else {
				this._setCheckedClass($parentNodes.first(), this.classNames.checked, false);
			}
		}
	},

    _setData: function(treeViewNodes) {
		if (treeViewNodes) {
			this._fixCheckedState(treeViewNodes);
		}
        this.$element.html(this.templates.treeView(treeViewNodes));
		//this._initializeTreeView();
    },
	
	_fixCheckedState: function(nodes) {
		for (var i = 0; i < nodes.length; i++) {
			this._fixCheckedStateOnSingleNode(nodes[i]);
		}
	},
	
	_fixCheckedStateOnSingleNode: function(node) {
		if (!node.children) {
			this._setCheckedState(node, node.isChecked);
			return;
		}
		
		if (node.isChecked) {
			// set all descendants to be checked
			this._setCheckedState(node, node.isChecked);
		}
		else {
			// first, fix checked status on children
			this._fixCheckedState(node.children);
		
			// check if children are checked
			var checkedChildrenCount = 0, uncheckedChildrenCount = 0;
			for (var i = 0; i < node.children.length; i++ ) {
				switch (node.children[i].checkedStatus) {
					case "checked":
						checkedChildrenCount++;
						break;
					case "unchecked":
						uncheckedChildrenCount++;
						break;
				}
			}
			
			if (checkedChildrenCount === node.children.length) {
				node.isChecked = true;
				node.checkedStatus = "checked";
				node.checkedClass = " "  + this.classNames.checked;
			}
			else if (uncheckedChildrenCount === node.children.length){
				node.isChecked = false;
				node.checkedStatus = "unchecked";
				node.checkedClass = " "  + this.classNames.unchecked;
			}
			else {
				node.isChecked = false;
				node.checkedStatus = "undetermined";
				node.checkedClass = " "  + this.classNames.undetermined;
			}
		}
	},
	
	_setCheckedState: function(node, isChecked) {
		node.isChecked = isChecked;
		node.checkedClass = isChecked ? " dj_tree_view_checked" : " dj_tree_view_unchecked";		
		node.checkedStatus = isChecked ? "checked" : "unchecked";
		
		if (node.children) {			
			for (var i = 0; i < node.children.length; i++) {
				this._setCheckedState(node.children[i], isChecked);
			}
		}
	},

    setData: function (treeViewNodes) {
        this._setData(treeViewNodes);
    },
	
	// Get data for the given nodes, get data for the whole tree if $nodes parameter is omitted
	getData: function ($nodes) {
		var childNodesSelector = "> " + this.selectors.treeContainer  + " > " + this.selectors.node;
	
		// if $nodes is null, getData from root
		if (!$nodes) {
			$nodes = this.$element.find(childNodesSelector);
		}
		
		var treeViewNodes = [];
		
		if ($nodes.length === 0) {
			return treeViewNodes;
		}
		
		for (var i = 0; i < $nodes.length; i++) {
			var $node = $nodes.eq(i);
			var treeViewNode = {
				text: $node.find("> " + this.selectors.nodeContent + " > " + this.selectors.text).html(),
				id: $node.data("id"),
				isOpen: $node.hasClass(this.classNames.open)
			};
			
			if ($node.hasClass(this.classNames.checked)) {
				treeViewNode.isChecked = true;
				treeViewNode.checkedStatus = "checked";
			}
			else if ($node.hasClass(this.classNames.undetermined)) {
				treeViewNode.isChecked = false;
				treeViewNode.checkedStatus = "undetermined";
			}
			else {
				treeViewNode.isChecked = false;
				treeViewNode.checkedStatus = "unchecked";
			}
			
			var $children = $node.find(childNodesSelector);
			if ($children.length > 0) {
				treeViewNode.children = this.getData($children);
			}
			
			treeViewNodes.push(treeViewNode);
		}		
		
		return treeViewNodes;
	},

    EOF: null
});

// Declare this class as a jQuery plugin
$.plugin('dj_TreeView', DJ.UI.TreeView);
$dj.debug('Registered DJ.UI.TreeView (extends DJ.UI.Component)');