using System.Collections.Generic;
using CreativeModeReborn.Components;
using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using KitchenMods;
using MessagePack;
using Unity.Collections;
using Unity.Entities;

namespace CreativeModeReborn.Views
{
    public class ApplianceMenuView : UpdatableObjectView<ApplianceMenuView.ViewData>
    {
        public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
        {
            private EntityQuery Views;
            private Dictionary<string, int> Appliances = new Dictionary<string, int>();
            protected override void Initialise()
            {
                base.Initialise();
                Views = GetEntityQuery(new QueryHelper().All(typeof(SApplianceMenuView), typeof(CLinkedView)));
                Appliances.Clear();
                foreach (Appliance appliance in GameData.Main.Get<Appliance>())
                {
                    if (!string.IsNullOrEmpty(appliance.name))
                        if (!Appliances.ContainsKey(appliance.name)) 
                            Appliances.Add(appliance.name, appliance.ID);
                }
            }
            protected override void OnUpdate()
            {
                NativeArray<CLinkedView> linkedViews = Views.ToComponentDataArray<CLinkedView>(Allocator.TempJob);
                foreach (CLinkedView linkedView in linkedViews)
                {
                    SendUpdate(linkedView.Identifier, new ViewData
                    {
                        Appliances = Appliances
                    });
                }
                linkedViews.Dispose();
            }
        }
        [MessagePackObject(false)]
        public struct ViewData : IViewData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(0)] public Dictionary<string, int> Appliances;
            public bool IsChangedFrom(ViewData cached)
            {
                return Appliances != cached.Appliances;
            }
        }
        
        protected override void UpdateData(ViewData view_data)
        {
            if (!transform.gameObject.HasComponent<Menu>())
                transform.gameObject.AddComponent<Menu>();
            
            Menu menu = transform.gameObject.GetComponent<Menu>();
            menu.Appliances = view_data.Appliances;
        }
    }
}