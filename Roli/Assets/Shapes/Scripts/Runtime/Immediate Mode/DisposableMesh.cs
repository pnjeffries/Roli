using System;
using System.Collections.Generic;
using UnityEngine;

// Shapes © Freya Holmér - https://twitter.com/FreyaHolmer/
// Website & Documentation - https://acegikmo.com/shapes/
namespace Shapes {

	public class DisposableMesh : IDisposable {

		static int activeMeshCount;
		public static int ActiveMeshCount => activeMeshCount;

		protected Mesh mesh;
		protected bool meshDirty = false;
		bool hasMesh = false; // used to detect if mesh needs to update on the fly on draw
		bool disposeWhenFullyReleased = false;

		internal List<DrawCommand> usedByCommands = null;

		protected void EnsureMeshExists() {
			if( hasMesh == false || mesh == null ) {
				mesh = new Mesh { hideFlags = HideFlags.DontSave };
				activeMeshCount++;
				hasMesh = true;
			}
		}

		// called when rendering a polyline
		internal void RegisterToCommandBuffer( DrawCommand cmd ) {
			if( usedByCommands == null ) {
				usedByCommands = ListPool<DrawCommand>.Alloc();
				Add();
			} else if( usedByCommands.Contains( cmd ) == false )
				Add();

			void Add() {
				usedByCommands.Add( cmd );
				cmd.cachedMeshes.Add( this );
			}
		}

		// called when a command is done rendering (or cleared)
		internal void ReleaseFromCommand( DrawCommand cmd ) {
			usedByCommands.Remove( cmd );
			if( usedByCommands.Count == 0 && disposeWhenFullyReleased )
				Dispose(); // now we can dispose this mesh data, it's no longer used
		}
		
		// • Draw.Polyline_Internal, Draw.Polygon_Internal calls RegisterToCommandBuffer if a buffer is being used
		// • if a Disposable mesh is disposed while registered, it will be marked for deletion later
		// • else if Dispose is called, it will delete/actually dispose the asset
		// • when a command buffer is done rendering, it will call ReleaseFromCommand(),
		// which will unregister and delete if no other buffers are using it
		public void Dispose() {
			disposeWhenFullyReleased = true; // when called inside a DrawCommand that still needs this mesh
			if( hasMesh ) {
				if( usedByCommands == null || usedByCommands.Count == 0 ) {
					ListPool<DrawCommand>.Free( usedByCommands );
					mesh.DestroyBranched();
					activeMeshCount--;
					hasMesh = false;
				}
			}
		}

		protected void ClearMesh() {
			if( hasMesh )
				mesh.Clear();
		}

		protected virtual bool ExternallyDirty() => false;
		protected virtual void UpdateMesh() => _ = 0;

		protected bool EnsureMeshIsReadyToRender( out Mesh outMesh, Action updateMesh ) {
			if( hasMesh == false ) {
				// no mesh exists because no points were added
				outMesh = null;
				return false;
			}

			if( meshDirty ) {
				updateMesh();
				meshDirty = false;
			}

			outMesh = mesh;
			return hasMesh;
		}

	}

}