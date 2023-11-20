import { mudConfig, resolveTableId } from "@latticexyz/world/register";

export default mudConfig({
  tables: {
    Position: {
      valueSchema: {
        x: "int32",
        y: "int32",
      },
    },
    Health: {
      valueSchema: {
        value: "uint32",
      },
    },
    Player: "bool",
    Damage: "uint32",
  },
  modules: [
    {
      name: "KeysWithValueModule",
      root: true,
      args: [resolveTableId("Position")],
    },
  ],
});
