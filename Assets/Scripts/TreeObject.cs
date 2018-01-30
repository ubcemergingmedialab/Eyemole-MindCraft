using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject {

	public TreeObject left;
	public TreeObject right;

	private float treeHeight = 0.25f;
	private float treeLength = 0.25f;
	public GameObject obj;

	public TreeObject(GameObject tree, Vector3 position, float angle) {

		left = null;
		right = null;
		obj = GameObject.Instantiate(tree);
		obj.transform.Rotate(Vector3.forward, angle, Space.World);
		obj.transform.position = position;

	}

	public TreeObject FindBranchLeft() {

		TreeObject leftmostObj = this;

		while (leftmostObj.left != null) {
			leftmostObj = leftmostObj.left;
		}

		return leftmostObj;

	}

	public TreeObject FindBranchRight() {

		TreeObject rightmostObj = this;

		while (rightmostObj.right != null) {
			rightmostObj = rightmostObj.right;
		}

		return rightmostObj;

	}

	public void AddBranchLeft(TreeObject parent, GameObject tree) {
		parent.left = new TreeObject(tree, parent.obj.transform.position + Vector3.up * treeHeight + Vector3.left * treeLength, 45f);
	}

	public void AddBranchRight(TreeObject parent, GameObject tree) {
		parent.right = new TreeObject(tree, parent.obj.transform.position + Vector3.up * treeHeight + Vector3.right * treeLength, -45f);

	}

	public void RemoveBranch(TreeObject branch) {

		branch.obj.SetActive(false);
		branch = null;
	}


}
