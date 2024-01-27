using CreativeModeReborn.Components;
using Kitchen;
using KitchenLib.Utils;
using KitchenMods;
using MessagePack;
using Unity.Collections;
using Unity.Entities;

namespace CreativeModeReborn.Views
{
    public class SpawnAppliancesView : ResponsiveObjectView<SpawnAppliancesView.ViewData, SpawnAppliancesView.ResponseData>
    {
        public class UpdateView : ResponsiveViewSystemBase<ViewData, ResponseData>, IModSystem
        {
            EntityQuery Query;

            protected override void Initialise()
            {
                base.Initialise();
                
                Query = GetEntityQuery(typeof(CLinkedView), typeof(SCreativeView));
            }

            protected override void OnUpdate()
            {
                using NativeArray<CLinkedView> linkedViews = Query.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                foreach (CLinkedView view in linkedViews)
                {
                    SendUpdate(view.Identifier, new ViewData());
                    
                    ApplyUpdates(view.Identifier, PerformUpdateWithResponse, only_final_update: true);
                }
            }

            private void PerformUpdateWithResponse(ResponseData data)
            {
                if (data.SpawnPassword == password)
                    SpawnUtils.SpawnApplianceBlueprint(data.SpawnID, 0, -1, false);
            }
        }

        [MessagePackObject(false)]
        public class ViewData : IViewData, IViewData.ICheckForChanges<ViewData>
        {
            public bool IsChangedFrom(ViewData check)
            {
                return true;
            }
        }
        [MessagePackObject(false)]
        public class ResponseData : IResponseData
        {
            [Key(0)] public FixedString32 SpawnPassword;
            [Key(1)] public int SpawnID;
        }

        protected override void UpdateData(ViewData data) { }

        public static int SpawnID = 0;
        public static string password = "";
        
        public override bool HasStateUpdate(out IResponseData state)
        {
            state = new ResponseData
            {
                SpawnPassword = password,
                SpawnID = SpawnID
            };
            
            if (SpawnID == 0)
                return false;
            
            SpawnID = 0;
            return true;
        }
    }
}