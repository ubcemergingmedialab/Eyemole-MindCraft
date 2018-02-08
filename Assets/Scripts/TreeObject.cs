using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject {

	public TreeObject left;
	public TreeObject right;
	public TreeObject parent;

	private float treeHeight = 0.4f;
	private float treeLength = 0.1f;
	private float scaleFactor = 1f;
	public GameObject obj;

	public TreeObject(GameObject tree, TreeObject parent, Vector3 position = default(Vector3), float angle = 0f) {

		left = null;
		right = null;

		obj = GameObject.Instantiate(tree);
		this.parent = parent;

		if (parent != null) {
			Transform parentTransform = parent.obj.transform;
			obj.transform.parent = parentTransform;
			obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
			obj.transform.localRotation = Quaternion.AngleAxis(angle, parentTransform.forward);
			obj.transform.localPosition = parentTransform.up * treeHeight * scaleFactor + parentTransform.right * treeHeight * Mathf.Sign(-1f * angle) * scaleFactor;

		} else {
			obj.transform.Rotate(Vector3.forward, angle, Space.World);
			obj.transform.position = position;
		}

	}

	public TreeObject GetRoot() {
		return this.parent == null ? this : this.parent.GetRoot();
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

	public TreeObject FindFirstBranchWithoutLeft() {

		TreeObject firstWithoutLeft = this;

		if (this.left == null) {
			return this;
		} else {
			Stack<TreeObject> stack;
			/* Points to node we are processing. */
			TreeObject traverse = this;
			/* Create a stack to hold node pointers. */
			stack = new Stack<TreeObject>();

			/*
			 * Gotta put something in the stack initially,
			 * so that we enter the body of the loop.
			 */
			stack.Push(this);

			while (stack.Count != 0) {

				traverse = stack.Pop();

				if (traverse.left == null)
					return traverse;
				else stack.Push(traverse.left);

				if (traverse.right != null)
					stack.Push(traverse.right);
			}

			stack.Clear();
		}

		return null;

	}

	public TreeObject FindFirstBranchWithoutRight() {

		TreeObject firstWithoutRight = this;

		if (this.right == null) {
			return this;
		} else {
			Stack<TreeObject> stack;
			/* Points to node we are processing. */
			TreeObject traverse = this;
			/* Create a stack to hold node pointers. */
			stack = new Stack<TreeObject>();

			/*
			 * Gotta put something in the stack initially,
			 * so that we enter the body of the loop.
			 */
			stack.Push(this);

			while (stack.Count != 0) {

				traverse = stack.Pop();

				if (traverse.right == null)
					return traverse;
				else stack.Push(traverse.right);

				if (traverse.left != null)
					stack.Push(traverse.left);
			}

			stack.Clear();
		}

		return null;

	}

	public void AddBranchLeft(TreeObject parent, GameObject tree) {
		parent.left = new TreeObject(tree, parent, parent.obj.transform.position + Vector3.up * treeHeight + Vector3.left * treeLength, 45f);
	}

	public void AddBranchRight(TreeObject parent, GameObject tree) {
		parent.right = new TreeObject(tree, parent, parent.obj.transform.position + Vector3.up * treeHeight + Vector3.right * treeLength, -45f);

	}

	public void RemoveBranch(TreeObject branch) {

		branch.obj.SetActive(false);
		branch = null;
	}


}
