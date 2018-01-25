using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject {

	public TreeObject left;
	public TreeObject right;

	public MeshRenderer rend;

	public TreeObject(GameObject tree) {
		left = null;
		right = null;
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


	}

	public void AddBranchRight(TreeObject parent, GameObject tree) {
	}

	public void RemoveBranchLeft(TreeObject parent, GameObject tree) {

	}

	public void RemoveBranchRight(TreeObject parent, GameObject tree) {

	}

}
