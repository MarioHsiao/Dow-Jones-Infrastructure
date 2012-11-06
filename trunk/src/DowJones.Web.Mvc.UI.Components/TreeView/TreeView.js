DJ.UI.TreeView = DJ.UI.Component.extend({
    defaults: {
        enableCheckboxes: false,
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

    _convertToJsTreeNodes: function (treeViewNodes) {
        if (!treeViewNodes)
            return null;

        var jsTreeNodes = [];

        for (var i = 0; i < treeViewNodes.length; i++) {
            var treeViewNode = treeViewNodes[i];
            var jsTreeNode = {
                data: treeViewNode.title,
                metadata: treeViewNode.metadata,
                children: this._convertToJsTreeNodes(treeViewNode.children)
            };

            if (treeViewNode.isChecked) {
                jsTreeNode.attr = {
                    "class": "jstree-checked"
                };
            }

            if (treeViewNode.isOpen) {
                jsTreeNode.state = "open";
            }

            jsTreeNodes.push(jsTreeNode);
        }
        return jsTreeNodes;
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
			var checkedChildrenCount = 0;
			for (var i = 0; i < node.children.length; i++ ) {
				if (node.children[i].isChecked) 
					checkedChildrenCount++;
			}
			
			if (checkedChildrenCount == node.children.length) {
				node.isChecked = true;
				node.checkedClass = " dj_tree_view_checked";
			}
			else {			
				node.isChecked = false;
				node.checkedClass = " dj_tree_view_undetermined";
			}
		}
	},
	
	_setCheckedState: function(node, isChecked) {
		node.isChecked = isChecked;
		node.checkedClass = isChecked ? " dj_tree_view_checked" : " dj_tree_view_unchecked";
		
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
				isOpen: $node.hasClass(this.classNames.open),
				isChecked: $node.hasClass(this.classNames.checked)
			};
			
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