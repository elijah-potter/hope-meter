import {json} from '@sveltejs/kit'
import state from "../../../state.json"

type HopeRecord = {
  unix_time: number,
  value: number,
}

type PersistantState = {
  history: HopeRecord[]
}

export async function GET(){
  state.history.sort((a, b) => a.unix_time - b.unix_time);

  return json({
    currentValue: (state.history.at(-1)?.value + 14) * 50
  })
}
