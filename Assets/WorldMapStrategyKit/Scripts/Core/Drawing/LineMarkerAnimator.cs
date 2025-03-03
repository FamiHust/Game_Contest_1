﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WorldMapStrategyKit {

    public class LineMarkerAnimator : MonoBehaviour {

        const int MINIMUM_POINTS = 64;
        // increase to improve line resolution

        public WMSK map;

        /// <summary>
        /// The list of map points to be traversed by the line.
        /// </summary>
        public Vector2[] path;

        /// <summary>
        /// The color of the line.
        /// </summary>
        public Color color;

        /// <summary>
        /// Line width (default: 0.01f)
        /// </summary>
        public float lineWidth = 0.01f;

        /// <summary>
        /// Arc of the line. A value of zero means the line will be drawn flat, on the ground.
        /// </summary>
        public float arcElevation;

        /// <summary>
        /// The duration of the drawing of the line. Zero means instant drawing.
        /// </summary>
        public float drawingDuration;

        /// <summary>
        /// The line material. If not supplied it will use the default lineMarkerMaterial.
        /// </summary>
        public Material lineMaterial;

        /// <summary>
        /// Specifies the duration in seconds for the line before it fades out
        /// </summary>
        public float autoFadeAfter = 0;

        /// <summary>
        /// The duration of the fade out.
        /// </summary>
        public float fadeOutDuration = 1.0f;

        /// <summary>
        /// 0 for continuous line.
        /// </summary>
        public float dashInterval;

        /// <summary>
        /// Duration of a cycle in seconds. 0.1f can be a good value. 0 = no animation.
        /// </summary>
        public float dashAnimationDuration = 0f;

        /// <summary>
        /// Number of points for the line. By default it will create a number of points based on path length and the MINIMUM_POINTS constant.
        /// </summary>
        public int numPoints;


        /// <summary>
        /// The starting line cap model or sprite.
        /// </summary>
        public GameObject startCap;

        /// <summary>
        /// Flips the cap
        /// </summary>
        public bool startCapFlipDirection;

        /// <summary>
        /// Scale for the start cap
        /// </summary>
        public Vector3 startCapScale = Misc.Vector3one;

        /// <summary>
        /// Offset for the start cap position
        /// </summary>
        public float startCapOffset = 0.1f;

        /// <summary>
        /// Optional material for the start cap
        /// </summary>
        public Material startCapMaterial;

        /// <summary>
        /// The end line cap model or sprite.
        /// </summary>
        public GameObject endCap;

        /// <summary>
        /// Flips the cap
        /// </summary>
        public bool endCapFlipDirection;

        /// <summary>
        /// Scale for the end cap
        /// </summary>
        public Vector3 endCapScale = Misc.Vector3one;

        /// <summary>
        /// Offset for the end cap position
        /// </summary>
        public float endCapOffset = 0.1f;

        /// <summary>
        /// Optional material for the end cap
        /// </summary>
        public Material endCapMaterial;


        /// <summary>
        /// Makes the line show in full at the start and reduce it progressively with time
        /// </summary>
        public bool reverseMode;


        /* Internal fields */
        float startTime, startAutoFadeTime;
        List<Vector3> vertices;
        LineRenderer lr;
        LineRenderer2 lrd;
        // for dashed lines
        Color colorTransparent;
        bool usesViewport;
        bool isFading;
        GameObject startCapPlaceholder, startCapGO;
        GameObject endCapPlaceholder, endCapGO;
        bool useArrowEndCap, useArrowStartCap;
        Vector3 startCapArrowTipPos, startCapArrowBasePos;
        Vector3 endCapArrowTipPos, endCapArrowBasePos;
        SpriteRenderer capSpriteRenderer;
        MeshRenderer capMeshRenderer;
        Color capStartColor;


        // Use this for initialization
        void Start() {

            startAutoFadeTime = float.MaxValue;
            colorTransparent = new Color(color.r, color.g, color.b, 0);

            if (arcElevation == 0 && drawingDuration == 0) {
                numPoints = path.Length;
            }

            // Compute path points on viewport or on 2D map
            usesViewport = map.renderViewportIsEnabled && arcElevation > 0;

            // Make line compatible with wrapping mode by offseting vertices according to the minimum distance - continous line are not supported due they use LineRenderer component and it can't skip segments
            if (map.wrapHorizontally) {
                if (usesViewport) {     // line is in viewport space and has elevation so just need to adjust x position
                    for (int k = 0; k < path.Length - 1; k++) {
                        float x0 = path[k].x;
                        float x1 = path[k + 1].x;
                        float dist = Mathf.Abs(x1 - x0);
                        if (1f - dist < dist) {
                            if (x1 > 0) {
                                path[k + 1].x -= 1f;
                            } else {
                                path[k + 1].x += 1f;
                            }
                        }
                    }
                }
            }

            // Create line vertices
            if (dashInterval > 0) {
                SetupDashedLine();
            } else {
                SetupLine();
            }

            useArrowStartCap = startCap != null;
            useArrowEndCap = endCap != null;

            Update();
        }


        // Update is called once per frame
        void Update() {
            UpdateLine();
            if (map.time >= startAutoFadeTime) {
                UpdateFade();
            }
        }


        void UpdateLine() {
            float t;

            if (drawingDuration == 0)
                t = 1.0f;
            else
                t = (map.time - startTime) / drawingDuration;
            if (t >= 1.0f) {
                t = 1.0f;
                if (autoFadeAfter == 0) {
                    if (!usesViewport && dashAnimationDuration == 0 && !isFading) {
                        enabled = false;    // disable this behaviour
                    }
                } else if (!isFading) {
                    startAutoFadeTime = map.time;
                    isFading = true;
                }
            }

            if (reverseMode) {
                t = 1f - t;
            }

            if (dashInterval > 0) {
                UpdateDashedLine(t);
            } else {
                UpdateContinousLine(t);
            }

            if (useArrowStartCap) {
                UpdateArrowCap(ref startCapPlaceholder, ref startCapGO, startCap, startCapFlipDirection, startCapArrowBasePos, startCapArrowTipPos, startCapScale, startCapOffset, startCapMaterial);
            }
            if (useArrowEndCap && endCapArrowTipPos != Misc.Vector3zero) {
                UpdateArrowCap(ref endCapPlaceholder, ref endCapGO, endCap, endCapFlipDirection, endCapArrowBasePos, endCapArrowTipPos, endCapScale, endCapOffset, endCapMaterial);
            }
        }

        void UpdateArrowCap(ref GameObject placeholder, ref GameObject obj, GameObject cap, bool flipDirection, Vector3 arrowBasePos, Vector3 arrowTipPos, Vector3 scale, float offset, Material capMaterial) {
            if (obj == null) {
                placeholder = new GameObject("CapPlaceholder");
                placeholder.transform.SetParent(transform, false);
                if (!usesViewport) {
                    placeholder.transform.localScale = new Vector3(1f / transform.lossyScale.x, 1f / transform.lossyScale.y, 1f);
                }
                obj = Instantiate<GameObject>(cap);
                capSpriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (capSpriteRenderer != null) {
                    capSpriteRenderer.color = lineMaterial.color;
                    capStartColor = capSpriteRenderer.color;
                }
                capMeshRenderer = obj.GetComponent<MeshRenderer>();
                if (capMeshRenderer != null) {
                    capMeshRenderer.sharedMaterial = capMaterial != null ? capMaterial : lineMaterial;
                    capStartColor = capMeshRenderer.sharedMaterial.color;
                }
                obj.transform.SetParent(placeholder.transform);
                obj.SetActive(false);
            }

            // set position
            Vector3 pos = Misc.Vector3zero;
            if (!usesViewport) {
                arrowBasePos = map.transform.TransformPoint(arrowBasePos);
                arrowTipPos = map.transform.TransformPoint(arrowTipPos);
            }

            // Length of cap based on size
            if (arrowBasePos != arrowTipPos) {
                Vector3 dir = Misc.Vector3zero;
                FastVector.NormalizedDirection(ref arrowBasePos, ref arrowTipPos, ref dir);
                pos = arrowTipPos - dir * offset;
                if (!obj.activeSelf) {
                    obj.SetActive(true);
                }
                obj.transform.position = pos;

                // look to camera
                if (usesViewport) {
                    Vector3 camDir = pos - map.cameraMain.transform.position;
                    obj.transform.LookAt(arrowTipPos + dir * 100f, camDir);
                } else {
                    Vector3 prdir = Vector3.ProjectOnPlane(dir, map.transform.forward);
                    obj.transform.LookAt(pos + map.transform.forward, prdir);
                    obj.transform.Rotate(obj.transform.forward, 90, Space.Self);
                }

                if (flipDirection) {
                    obj.transform.Rotate(180, 0, 0, Space.Self);
                }

            }
            obj.transform.localScale = scale;

        }


        void UpdateFade() {
            float t = map.time - startAutoFadeTime;
            if (t < autoFadeAfter)
                return;

            t = (t - autoFadeAfter) / fadeOutDuration;
            if (t >= 1.0f) {
                t = 1.0f;
                Destroy(gameObject);
            }

            Color fadeColor = Color.Lerp(color, colorTransparent, t);
            lineMaterial.color = fadeColor;

            if (capSpriteRenderer != null) {
                capSpriteRenderer.color = Color.Lerp(capStartColor, colorTransparent, t);
            }

            if (capMeshRenderer != null) {
                capMeshRenderer.sharedMaterial.color = Color.Lerp(capStartColor, colorTransparent, t);
            }

        }

        /// <summary>
        /// Fades out current line.
        /// </summary>
        public void FadeOut(float duration) {
            startAutoFadeTime = map.time;
            fadeOutDuration = duration;
            isFading = true;
            enabled = true;
        }


        #region Continous line

        void SetupLine() {
            // Create the line mesh
            if (numPoints <= 0)
                numPoints = Mathf.Max(MINIMUM_POINTS, path.Length - 1);
            startTime = map.time;
            if (!usesViewport) {
                arcElevation *= 100f;
            }
            lr = transform.GetComponent<LineRenderer>();
            if (lr == null) {
                lr = gameObject.AddComponent<LineRenderer>();
            }
            lr.useWorldSpace = usesViewport;
            lineMaterial = Instantiate(lineMaterial);
            lineMaterial.color = color;
            lr.material = lineMaterial; // needs to instantiate to preserve individual color so can't use sharedMaterial
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
        }

        void CreateLineVertices() {

            if (vertices == null) {
                vertices = new List<Vector3>(numPoints + 1);
            } else {
                vertices.Clear();
            }

            float elevationStart = 0, elevationEnd = 0;
            if (usesViewport) {
                lineWidth *= 6.0f;
                elevationStart = map.ComputeEarthHeight(path[0], false);
                elevationEnd = map.ComputeEarthHeight(path[path.Length - 1], false);
            } 

            Vector3 mapPos;
            for (int s = 0; s <= numPoints; s++) {
                float t = (float)s / numPoints;
                int index = (int)((path.Length - 1) * t);
                int findex = Mathf.Min(index + 1, path.Length - 1);
                float t0 = t * (path.Length - 1);
                t0 -= index;
                mapPos = Vector2.Lerp(path[index], path[findex], t0);
                if (usesViewport) {
                    if (map.renderViewportRect.Contains(map.Map2DToRenderViewport(mapPos))) {
                        float elevation = Mathf.Lerp(elevationStart, elevationEnd, t);
                        elevation += arcElevation > 0 ? Mathf.Sin(t * Mathf.PI) * arcElevation : 0;
                        mapPos = map.Map2DToWorldPosition(mapPos, elevation, HEIGHT_OFFSET_MODE.ABSOLUTE_CLAMPED, false);
                        vertices.Add(mapPos);
                    }
                } else {
                    if (arcElevation > 0) {
                        mapPos.z = -Mathf.Sin(t0 * Mathf.PI) * arcElevation;
                    }
                    vertices.Add(mapPos);
                }
            }
        }

        void UpdateContinousLine(float t) {
            CreateLineVertices();

            int vertexCount = vertices.Count;
            float vertexIndex = 1 + (vertexCount - 2) * t;
            int currentVertex = (int)(vertexIndex);
            lr.positionCount = currentVertex + 1;
            if (currentVertex >= 0 && currentVertex < vertexCount) {
                for (int k = 0; k < currentVertex; k++) {
                    lr.SetPosition(k, vertices[k]);
                }
                // adjust last segment
                Vector3 nextVertexPos = vertices[currentVertex];
                Vector3 currentVertexPos = vertices[currentVertex > 0 ? currentVertex - 1 : 0];
                float subt = vertexIndex - currentVertex;
                Vector3 progress = t >= 1f ? nextVertexPos : Vector3.Lerp(currentVertexPos, nextVertexPos, subt);
                lr.SetPosition(currentVertex, progress);

                // set line cap positions
                startCapArrowTipPos = vertices[0];
                endCapArrowTipPos = progress;
                if (vertexCount > 1) {
                    startCapArrowBasePos = vertices[1];
                    endCapArrowBasePos = currentVertexPos;
                } else {
                    startCapArrowBasePos = startCapArrowTipPos;
                    endCapArrowBasePos = endCapArrowTipPos;
                }
            }
        }

        #endregion

        #region Dashed line

        void SetupDashedLine() {
            // Create the line mesh
            startTime = map.time;
            if (!usesViewport) {
                arcElevation *= 100f;
            }
            lrd = transform.GetComponent<LineRenderer2>();
            if (lrd == null) {
                lrd = gameObject.AddComponent<LineRenderer2>();
            }
            lrd.useWorldSpace = usesViewport; // needed since thickness should be independent of parent scale
            if (!usesViewport) {
                lineWidth /= map.transform.localScale.x;
            }
            lineMaterial = Instantiate(lineMaterial);
            lineMaterial.color = color;
            lrd.material = lineMaterial; // needs to instantiate to preserve individual color so can't use sharedMaterial
            lrd.SetColors(color, color);
            lrd.SetWidth(lineWidth, lineWidth);
        }

        void CreateDashedLineVertices() {

            // Prepare elevation range
            float elevationStart = 0, elevationEnd = 0;
            if (usesViewport) {
                lineWidth *= 6.0f;
                elevationStart = map.ComputeEarthHeight(path[0], false);
                elevationEnd = map.ComputeEarthHeight(path[path.Length - 1], false);
            }

            // Calculate total line distance
            float totalDistance = 0;
            Vector2 prev = Misc.Vector2zero;
            int max = path.Length - 1;
            for (int s = 0; s <= max; s++) {
                Vector2 current = path[s];
                if (s > 0) {
                    totalDistance += Vector2.Distance(current, prev);
                }
                prev = current;
            }

            // Dash animation?
            float startingDistance = 0;
            float step = dashInterval * 2f;
            if (dashAnimationDuration > 0) {
                float ett = map.time / dashAnimationDuration;
                float elapsed = ett - (int)ett;
                startingDistance = elapsed * step;
            }

            // Compute dash segments
            if (vertices == null) {
                vertices = new List<Vector3>(100);
            } else {
                vertices.Clear();
            }

            if (totalDistance == 0)
                return;

            int pair = 0;
            Vector3 mapPos;
            for (float distanceAcum = startingDistance; distanceAcum < totalDistance + step; distanceAcum += dashInterval, pair++) {
                float t0 = Mathf.Clamp01(distanceAcum / totalDistance);
                float t = t0 * (path.Length - 1);
                int index = (int)t;
                int findex = Mathf.Min(index + 1, path.Length - 1);

                t -= index;
                if (index < 0 || index >= path.Length || findex < 0 || findex >= path.Length)
                    continue;

                mapPos = Vector2.Lerp(path[index], path[findex], t);

                if (usesViewport) {
                    if (map.renderViewportRect.Contains(map.Map2DToRenderViewport(mapPos))) {
                        float elevation = Mathf.Lerp(elevationStart, elevationEnd, t0);
                        elevation += arcElevation > 0 ? Mathf.Sin(t0 * Mathf.PI) * arcElevation : 0;
                        Vector3 sPos = map.Map2DToWorldPosition(mapPos, elevation, HEIGHT_OFFSET_MODE.ABSOLUTE_CLAMPED, false);
                        if (vertices.Count > 0 || (pair % 2 == 0)) {
                            vertices.Add(sPos);
                        }
                    }
                } else {
                    if (arcElevation > 0) {
                        mapPos.z = -Mathf.Sin(t0 * Mathf.PI) * arcElevation;
                    }
                    vertices.Add(mapPos);
                }
            }
        }


        void UpdateDashedLine(float t) {
            // pass current vertices
            CreateDashedLineVertices();

            int vertexCount = vertices.Count;
            float vertexIndex = 1f + (vertexCount - 2) * t;
            int currentVertex = (int)(vertexIndex);
            lrd.SetVertexCount(currentVertex + 1);
            if (currentVertex >= 0 && currentVertex < vertexCount) {
                for (int k = 0; k < currentVertex; k++) {
                    lrd.SetPosition(k, vertices[k]);
                }

                // adjust last segment
                Vector3 nextVertexPos = vertices[currentVertex];
                Vector3 progress;
                if (t >= 1) {
                    progress = nextVertexPos;
                } else {
                    Vector3 currentVertexPos = vertices[currentVertex > 0 ? currentVertex - 1 : 0];
                    float subt = vertexIndex - currentVertex;
                    progress = Vector3.Lerp(currentVertexPos, nextVertexPos, subt);
                }
                lrd.SetPosition(currentVertex, progress);

                // set line cap positions
                startCapArrowTipPos = vertices[0];
                if (vertexCount > 1) {
                    startCapArrowBasePos = vertices[1];
                } else {
                    startCapArrowBasePos = startCapArrowTipPos;
                }

                if (useArrowEndCap && t > 0.1f) {
                    t += 0.1f;
                    if (t > 1f)
                        t = 1f;
                    float elevationStart = 0, elevationEnd = 0;
                    if (usesViewport) {
                        lineWidth *= 6.0f;
                        elevationStart = map.ComputeEarthHeight(path[0], false);
                        elevationEnd = map.ComputeEarthHeight(path[path.Length - 1], false);
                    }
                    int index = (int)((path.Length - 1) * t);
                    int findex = Mathf.Min(index + 1, path.Length - 1);
                    float t0 = t * (path.Length - 1);
                    t0 -= index;
                    Vector3 mapPos = Vector2.Lerp(path[index], path[findex], t0);
                    if (usesViewport) {
                        if (map.renderViewportRect.Contains(map.Map2DToRenderViewport(mapPos))) {
                            float elevation = Mathf.Lerp(elevationStart, elevationEnd, t);
                            elevation += arcElevation > 0 ? Mathf.Sin(t * Mathf.PI) * arcElevation : 0;
                            mapPos = map.Map2DToWorldPosition(mapPos, elevation, HEIGHT_OFFSET_MODE.ABSOLUTE_CLAMPED, false);
                        }
                    } else {
                        if (arcElevation > 0) {
                            mapPos.z = -Mathf.Sin(t0 * Mathf.PI) * arcElevation;
                        }
                    }
                    endCapArrowBasePos = endCapArrowTipPos;
                    endCapArrowTipPos = mapPos;
                }
            }
        }

        #endregion

    }
}