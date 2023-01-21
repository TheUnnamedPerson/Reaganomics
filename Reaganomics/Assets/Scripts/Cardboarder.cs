using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(PolygonCollider2D))]
public class Cardboarder : MonoBehaviour {

	public float TextureTileScale = 1f;
	public float CardboardTickness = 1f;
	public Sprite backSprite;
	public SpriteLibraryAsset backLibrary;
	public bool DoubleSided = true;
	public bool DuplicateSpriteOnBackFace = true;
	public bool invertMesh = false;
	public Material CardboardLineMaterial;
	public Material CardboardFaceMaterial;
	public Color FaceFillColor;

	private List<Vector2> points = new List<Vector2>();
	private List<Vector2> simplifiedPoints = new List<Vector2>();
	
	void UpdatePolygonCollider2D(float tolerance = 0.005f)
	{
		PolygonCollider2D polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
		SpriteRenderer originalSpriteRenderer = GetComponent<SpriteRenderer>();
		polygonCollider2D.pathCount = originalSpriteRenderer.sprite.GetPhysicsShapeCount();
		for(int i = 0; i < polygonCollider2D.pathCount; i++)
		{
			originalSpriteRenderer.sprite.GetPhysicsShape(i, points);
			LineUtility.Simplify(points, tolerance, simplifiedPoints);
			polygonCollider2D.SetPath(i, simplifiedPoints);
		}
	}

	void Start()
	{
		CardBoardify();
	}

	public void CardBoardify()
	{
		UpdatePolygonCollider2D();
		foreach(Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		CreateCardboardSideLineMesh();
		CreateCardboardFace(0.01f);

		if(DoubleSided)
			CreateCardboardFace(CardboardTickness - 0.01f, true);

		if (DuplicateSpriteOnBackFace)
		{
			SpriteRenderer originalSpriteRenderer = GetComponent<SpriteRenderer>();
			if(originalSpriteRenderer != null)
			{
				GameObject obj = new GameObject("BackFaceSprite");
				obj.transform.parent = transform;
				obj.transform.localScale = Vector3.one;
				obj.transform.localEulerAngles = Vector3.zero;
				obj.transform.localPosition = new Vector3(0, 0, CardboardTickness);

				SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = originalSpriteRenderer.sprite;
				spriteRenderer.color = originalSpriteRenderer.color;
				spriteRenderer.flipX = originalSpriteRenderer.flipX;
				spriteRenderer.flipY = originalSpriteRenderer.flipY;
			}
		}
		else
		{
			SpriteRenderer originalSpriteRenderer = GetComponent<SpriteRenderer>();
			if(originalSpriteRenderer != null)
			{
				GameObject obj = new GameObject("BackFaceSprite");
				obj.transform.parent = transform;
				obj.transform.localScale = Vector3.one;
				obj.transform.localEulerAngles = Vector3.zero;
				obj.transform.localPosition = new Vector3(0, 0, CardboardTickness);

				SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = backSprite;
				spriteRenderer.color = originalSpriteRenderer.color;
				spriteRenderer.flipX = originalSpriteRenderer.flipX;
				spriteRenderer.flipY = originalSpriteRenderer.flipY;

				SpriteLibrary sLib = obj.AddComponent<SpriteLibrary>();
				sLib.spriteLibraryAsset = backLibrary;
				SpriteResolver sRes = obj.AddComponent<SpriteResolver>();
				sRes.SetCategoryAndLabel(GetComponent<SpriteResolver>().GetCategory(), GetComponent<SpriteResolver>().GetLabel());
			}
		}
	}

	private void CreateCardboardSideLineMesh()
	{
		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> triangles = new List<int>();

		float totalDistance = 0;
		PolygonCollider2D col = gameObject.GetComponent<PolygonCollider2D>();
		for (int i = 0; i < col.points.Length; i++)
		{
			Vector2 current = col.points[i];
			Vector2 next = col.points[(i + 1) % col.points.Length];

			vertices.Add(new Vector3(current.x, current.y));
			vertices.Add(new Vector3(next.x, next.y));
			vertices.Add(new Vector3(next.x, next.y, CardboardTickness));
			vertices.Add(new Vector3(current.x, current.y, CardboardTickness));

			float distance = TextureTileScale * Vector2.Distance(current, next);
			float rest = totalDistance - Mathf.Floor(totalDistance);
			totalDistance += distance;

			uvs.Add(new Vector2(rest, 0));
			uvs.Add(new Vector2(rest + distance, 0));
			uvs.Add(new Vector2(rest + distance, 1));
			uvs.Add(new Vector2(rest, 1));

			int startIndex = i * 4;
			triangles.Add(startIndex);
			triangles.Add(startIndex + 1);
			triangles.Add(startIndex + 2);
			triangles.Add(startIndex);
			triangles.Add(startIndex + 2);
			triangles.Add(startIndex + 3);
		}

		if (invertMesh) triangles.Reverse();

		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = vertices.ToArray();
		m.uv = uvs.ToArray();
		m.triangles = triangles.ToArray();
		m.RecalculateNormals();

		GameObject obj = new GameObject("CardboardLine");
		obj.transform.parent = transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localEulerAngles = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
		MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
		meshFilter.mesh = m;
		renderer.material = CardboardLineMaterial;
	}

	private void CreateCardboardFace(float zOffset, bool invert = false)
	{
		float minX, maxX, minY, maxY;
		minX = float.MaxValue;
		maxX = float.MinValue;
		minY = float.MaxValue;
		maxY = float.MinValue;

		PolygonCollider2D col = gameObject.GetComponent<PolygonCollider2D>();
		List<Vector3> polyList = new List<Vector3>();
		for (int i = 0; i < col.points.Length; i++)
		{
			Vector2 pos = new Vector2(col.points[i].x, col.points[i].y);
			polyList.Add(pos);

			if (pos.x < minX) minX = pos.x;
			if (pos.x > maxX) maxX = pos.x;

			if (pos.y < minY) minY = pos.y;
			if (pos.y > maxY) maxY = pos.y;
		}
		Vector3[] poly = polyList.ToArray();

		int[] indices = Triangulate(poly);

		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = poly;
		msh.triangles = indices;

		if(invert)
		{
			List<int> test = new List<int>();
			test.AddRange(msh.triangles);
			test.Reverse();
			msh.triangles = test.ToArray();
		}

		msh.RecalculateNormals();
		msh.RecalculateBounds();

		// Set up game object with mesh;
		GameObject obj = new GameObject("CardboardFace");
		obj.transform.parent = transform;
		obj.transform.localEulerAngles = Vector3.zero;
		obj.transform.localPosition = new Vector3(0, 0, zOffset);
		obj.transform.localScale = Vector3.one;
		MeshRenderer rend = obj.AddComponent<MeshRenderer>();
		MeshFilter filter = obj.AddComponent<MeshFilter>();
		filter.mesh = msh;
		filter.sharedMesh.uv = BuildUVs(poly);

		// Set material
		if (CardboardFaceMaterial != null)
			rend.sharedMaterial = CardboardFaceMaterial;
		rend.sharedMaterial.color = FaceFillColor;
		float materialTilingRatio = 1f;
		float xTiling = (maxX - minX) * materialTilingRatio;
		float yTiling = (maxY - minY) * materialTilingRatio;
		rend.sharedMaterial.mainTextureScale = new Vector2(xTiling, yTiling);

		float xOffset = (minX * materialTilingRatio - Mathf.Floor(minX * materialTilingRatio));
		float yOffset = (minY * materialTilingRatio - Mathf.Floor(minY * materialTilingRatio));

		rend.sharedMaterial.mainTextureOffset = new Vector2(xOffset, yOffset);
	}

	protected Vector3 FindCenter(Vector3[] poly)
	{
		Vector3 center = Vector3.zero;
		foreach (Vector3 v3 in poly)
		{
			center += v3;
		}
		return center / poly.Length;
	}

	protected Vector2[] BuildUVs(Vector3[] vertices)
	{
		float xMin = Mathf.Infinity;
		float yMin = Mathf.Infinity;
		float xMax = -Mathf.Infinity;
		float yMax = -Mathf.Infinity;

		foreach (Vector3 v3 in vertices)
		{
			if (v3.x < xMin)
				xMin = v3.x;
			if (v3.y < yMin)
				yMin = v3.y;
			if (v3.x > xMax)
				xMax = v3.x;
			if (v3.y > yMax)
				yMax = v3.y;
		}

		float xRange = xMax - xMin;
		float yRange = yMax - yMin;

		Vector2[] uvs = new Vector2[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			uvs[i].x = (vertices[i].x - xMin) / xRange;
			uvs[i].y = (vertices[i].y - yMin) / yRange;

		}
		return uvs;
	}

	protected int[] Triangulate(Vector3[] poly)
	{
		List<int> indices = new List<int>();

		int n = poly.Length;
		if (n < 3)
			return indices.ToArray();

		int[] V = new int[n];
		if (Area(poly) > 0)
		{
			for (int v = 0; v < n; v++)
				V[v] = v;
		}
		else
		{
			for (int v = 0; v < n; v++)
				V[v] = (n - 1) - v;
		}

		int nv = n;
		int count = 2 * nv;
		for (int m = 0, v = nv - 1; nv > 2;)
		{
			if ((count--) <= 0)
				return indices.ToArray();

			int u = v;
			if (nv <= u)
				u = 0;
			v = u + 1;
			if (nv <= v)
				v = 0;
			int w = v + 1;
			if (nv <= w)
				w = 0;

			if (Snip(poly, u, v, w, nv, V))
			{
				int a, b, c, s, t;
				a = V[u];
				b = V[v];
				c = V[w];
				indices.Add(a);
				indices.Add(b);
				indices.Add(c);
				m++;
				for (s = v, t = v + 1; t < nv; s++, t++)
					V[s] = V[t];
				nv--;
				count = 2 * nv;
			}
		}

		indices.Reverse();
		return indices.ToArray();
	}

	public float Area(Vector3[] poly)
	{
		int n = poly.Length;
		float A = 0.0f;
		for (int p = n - 1, q = 0; q < n; p = q++)
		{
			Vector2 pval = poly[p];
			Vector2 qval = poly[q];
			A += pval.x * qval.y - qval.x * pval.y;
		}
		return (A * 0.5f);
	}

	protected bool Snip(Vector3[] poly, int u, int v, int w, int n, int[] V)
	{
		int p;
		Vector2 A = poly[V[u]];
		Vector2 B = poly[V[v]];
		Vector2 C = poly[V[w]];
		if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
			return false;
		for (p = 0; p < n; p++)
		{
			if ((p == u) || (p == v) || (p == w))
				continue;
			Vector2 P = poly[V[p]];
			if (InsideTriangle(A, B, C, P))
				return false;
		}
		return true;
	}

	protected bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
	{
		float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
		float cCROSSap, bCROSScp, aCROSSbp;

		ax = C.x - B.x; ay = C.y - B.y;
		bx = A.x - C.x; by = A.y - C.y;
		cx = B.x - A.x; cy = B.y - A.y;
		apx = P.x - A.x; apy = P.y - A.y;
		bpx = P.x - B.x; bpy = P.y - B.y;
		cpx = P.x - C.x; cpy = P.y - C.y;

		aCROSSbp = ax * bpy - ay * bpx;
		cCROSSap = cx * apy - cy * apx;
		bCROSScp = bx * cpy - by * cpx;

		return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
	}
}
