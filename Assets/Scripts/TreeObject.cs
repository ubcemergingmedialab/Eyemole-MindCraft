using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject {

	public TreeObject left;
	public TreeObject right;

	public GameObject obj;

	public TreeObject(GameObject tree) {

		left = null;
		right = null;
		obj = tree;
		GameObject.Instantiate(tree);
	}

	public TreeObject FindBranchLeft() {

		TreeObject leftmostObj = this;

		while (leftmostObj != null) {
			leftmostObj = leftmostObj.left;
		}

		return leftmostObj;

	}

	public TreeObject FindBranchRight() {

		TreeObject rightmostObj = this;

		while (rightmostObj != null) {
			rightmostObj = rightmostObj.right;
		}

		return rightmostObj;

	}

	public void AddBranchLeft(TreeObject parent, GameObject tree) {

		tree.transform.parent = parent.obj.transform;
		parent.left = new TreeObject(tree);
	}

	public void AddBranchRight(TreeObject parent, GameObject tree) {

		tree.transform.parent = parent.obj.transform;
		parent.right = new TreeObject(tree);

	}

	public void RemoveBranchLeft(TreeObject parent) {

		parent.left = null;
	}

	public void RemoveBranchRight(TreeObject parent) {

		parent.right = null;
	}

}
