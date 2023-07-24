/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud.Client;
using mud.Network.schemas;
using mud.Unity;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace DefaultNamespace
{
    public class PositionTableUpdate : TypedRecordUpdate<Tuple<PositionTable?, PositionTable?>> { }

    public class PositionTable : IMudTable
    {
        public static readonly TableId TableId = new("", "Position");

        public long? x;
        public long? y;

        public static IObservable<PositionTableUpdate> OnRecordUpdate()
        {
            return NetworkManager.Instance.ds.OnDataStoreUpdate
                .Where(
                    update =>
                        update.TableId == TableId.ToString() && update.Type == UpdateType.SetField
                )
                .Select(
                    update =>
                        new PositionTableUpdate
                        {
                            TableId = update.TableId,
                            Key = update.Key,
                            Value = update.Value,
                            TypedValue = MapUpdates(update.Value)
                        }
                );
        }

        public static IObservable<PositionTableUpdate> OnRecordInsert()
        {
            return NetworkManager.Instance.ds.OnDataStoreUpdate
                .Where(
                    update =>
                        update.TableId == TableId.ToString() && update.Type == UpdateType.SetRecord
                )
                .Select(
                    update =>
                        new PositionTableUpdate
                        {
                            TableId = update.TableId,
                            Key = update.Key,
                            Value = update.Value,
                            TypedValue = MapUpdates(update.Value)
                        }
                );
        }

        public static IObservable<PositionTableUpdate> OnRecordDelete()
        {
            return NetworkManager.Instance.ds.OnDataStoreUpdate
                .Where(
                    update =>
                        update.TableId == TableId.ToString()
                        && update.Type == UpdateType.DeleteRecord
                )
                .Select(
                    update =>
                        new PositionTableUpdate
                        {
                            TableId = update.TableId,
                            Key = update.Key,
                            Value = update.Value,
                            TypedValue = MapUpdates(update.Value)
                        }
                );
        }

        public static Tuple<PositionTable?, PositionTable?> MapUpdates(
            Tuple<Property?, Property?> value
        )
        {
            PositionTable? current = null;
            PositionTable? previous = null;

            if (value.Item1 != null)
            {
                try
                {
                    current = new PositionTable
                    {
                        x = value.Item1.TryGetValue("x", out var xVal) ? (long)xVal : default,
                        y = value.Item1.TryGetValue("y", out var yVal) ? (long)yVal : default,
                    };
                }
                catch (InvalidCastException)
                {
                    current = new PositionTable { x = null, y = null, };
                }
            }

            if (value.Item2 != null)
            {
                try
                {
                    previous = new PositionTable
                    {
                        x = value.Item2.TryGetValue("x", out var xVal) ? (long)xVal : default,
                        y = value.Item2.TryGetValue("y", out var yVal) ? (long)yVal : default,
                    };
                }
                catch (InvalidCastException)
                {
                    previous = new PositionTable { x = null, y = null, };
                }
            }

            return new Tuple<PositionTable?, PositionTable?>(current, previous);
        }
    }
}